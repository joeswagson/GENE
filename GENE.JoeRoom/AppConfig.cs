using Microsoft.Extensions.Configuration;

namespace GENE.JoeRoom;

static class AppConfig
{
    private static readonly Lazy<IConfiguration> Cfg = new(() =>
        new ConfigurationBuilder()
            .AddUserSecrets<RoomCluster>()
            .Build()
    );

    internal static IConfiguration Secrets => Cfg.Value;
}
