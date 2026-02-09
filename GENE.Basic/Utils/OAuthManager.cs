using System.Text.Json;
using GENE.Basic.Nodes.SmartThings;

namespace GENE.JoeRoom;

public sealed class OAuthManager(
    string clientId,
    string clientSecret,
    string redirectUri,
    HttpClient? httpClient = null)
    : IDisposable
{
    private readonly HttpClient _http = httpClient ?? new HttpClient();

    private string? _accessToken;
    private string? _refreshToken;
    private DateTimeOffset _expiresAt;

    private static readonly Uri TokenEndpoint =
        new("https://api.smartthings.com/oauth/token");

    /// Call once after redirect, when you receive ?code=...
    public async Task ExchangeCodeAsync(string authorizationCode, CancellationToken ct = default)
    {
        var form = new FormUrlEncodedContent([
            new("grant_type", "authorization_code"),
            new("code", authorizationCode),
            new("client_id", clientId),
            new("client_secret", clientSecret),
            new("redirect_uri", redirectUri)
        ]);

        using var resp = await _http.PostAsync(TokenEndpoint, form, ct);
        resp.EnsureSuccessStatusCode();

        await ReadTokenResponseAsync(resp, ct);
    }

    public SmartThingsToken GetToken() => GetTokenAsync().GetAwaiter().GetResult();

    public async Task<SmartThingsToken> GetTokenAsync(CancellationToken ct = default)
    {
        if (_accessToken != null && DateTimeOffset.UtcNow < _expiresAt) return new(_accessToken!);
        if (_refreshToken == null)
            throw new InvalidOperationException("No refresh token available");

        await RefreshAsync(ct);

        return new(_accessToken!);
    }

    private async Task RefreshAsync(CancellationToken ct)
    {
        var form = new FormUrlEncodedContent([
            new("grant_type", "refresh_token"),
            new("refresh_token", _refreshToken!),
            new("client_id", clientId),
            new("client_secret", clientSecret)
        ]);

        using var resp = await _http.PostAsync(TokenEndpoint, form, ct);
        resp.EnsureSuccessStatusCode();

        await ReadTokenResponseAsync(resp, ct);
    }

    private async Task ReadTokenResponseAsync(HttpResponseMessage resp, CancellationToken ct)
    {
        await using var stream = await resp.Content.ReadAsStreamAsync(ct);
        using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

        var root = doc.RootElement;

        _accessToken = root.GetProperty("access_token").GetString();
        _refreshToken = root.GetProperty("refresh_token").GetString();

        var expiresIn = root.GetProperty("expires_in").GetInt32();
        _expiresAt = DateTimeOffset.UtcNow.AddSeconds(expiresIn - 30); // safety margin
    }

    public void Dispose()
    {
        _http.Dispose();
    }
}