using System.Security.Cryptography;
using System.Text.Json.Nodes;

namespace GENE.Flow.Daemon.Protocol.JSON;

public delegate IFlowMessage UnpackageDelegate(JsonObject src);
public interface IFlowMessage
{
    public string Name { get; }

    void Package(JsonObject self);
    static abstract IFlowMessage Unpackage(JsonObject src);
    void Recieved();
    
    
    
    public JsonObject Encode()
    {
        var data = new JsonObject();
        var array = new JsonObject {
            ["name"] = Name,
            ["data"] = data
        };

        Package(data);
        return array;
    }

    public T Decode<T>(JsonObject src) where  T : IFlowMessage
    {
        return (T) T.Unpackage(src);
    }
}