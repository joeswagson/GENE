using System.Text.Json.Nodes;
using GENE.Flow.Extensions;

namespace GENE.Flow.Daemon.Protocol.JSON.Communication;

public class Ack(string message) : IFlowMessage
{
    public string Name => "Ack";
    void IFlowMessage.Package(JsonObject self) => self["message"] = message;
    static IFlowMessage IFlowMessage.Unpackage(JsonObject src) => new Ack(src.Required<string>("message"));

    void IFlowMessage.Recieved()
    {
    }
}