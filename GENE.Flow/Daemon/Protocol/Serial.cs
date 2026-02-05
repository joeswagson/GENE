using System.Text;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using GENE.Flow.Daemon.Protocol.JSON;
using GENE.Flow.Extensions;

namespace GENE.Flow.Daemon.Protocol;

public class Serial
{
    public static readonly Encoding DefaultEncoding = Encoding.UTF8;

    public static Dictionary<string, UnpackageDelegate>? Decoders { get; private set; }
    private static readonly HashSet<string> ValidRoots = [];

    public static bool PacketsCached => Decoders != null;

    public static void RegisterMesssageNamespace(string @namespace)
    {
        ValidRoots.Add(@namespace);
    }

    public static void Invalidate() => Decoders = null;

    public static bool Populate()
    {
        if (PacketsCached)
            return false;

        Decoders = GetMessageDecoders();
        return true;
    }

    public static void ReloadCache()
    {
        Invalidate();
        Populate();
    }

    private static Dictionary<string, UnpackageDelegate> GetMessageDecoders()
    {
        var dict = new Dictionary<string, UnpackageDelegate>(StringComparer.Ordinal);

        var types = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                t.Namespace != null &&
                ValidRoots.Any(root => t.Namespace.StartsWith(root, StringComparison.Ordinal)) &&
                typeof(IFlowMessage).IsAssignableFrom(t)
            )
            .OrderBy(t => t.FullName, StringComparer.Ordinal);

        foreach (var type in types)
            dict[type.FullName!] = (j) => (IFlowMessage)type.GetMethod(
                    nameof(IFlowMessage.Unpackage),
                    [typeof(JsonArray)])?
                .Invoke(null, [j])!;

        return dict;
    }


    public static string Encode(IFlowMessage message) => message.Encode().ToJsonString();

    public static IFlowMessage Decode(string msg)
    {
        var json = (JsonObject?)JsonNode.Parse(msg);
        if (json is null)
            throw new ArgumentNullException(nameof(msg), "Failed to deserialize incoming message.");

        var name = json.Required<string>("name");
        if (!Decoders?.ContainsKey(name) ?? true)
            throw new InvalidDataException($"Could not find decoder for message type \"{name}\"");

        return Decoders[name](json);
    }
}