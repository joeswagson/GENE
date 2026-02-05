using GENE.CLI.Commands.Types;
using GENE.Flow;
using GENE.Flow.Daemon;
using GENE.Flow.Daemon.Protocol;
using GENE.Flow.Daemon.Protocol.JSON.Communication;

namespace GENE.CLI.Commands;

public enum FlowCommandAction
{
    ack,
    daemon
}
public class FlowCommand : Command
{
    public override string Identifier => "flow";
    public override Usage Help() => new("flow", "Interface with the Flow backend and app.");

    public override void Execute(string[] args)
    {
        var cmd = Required(0, Enum.Parse<FlowCommandAction>);
        switch (cmd)
        {
            case FlowCommandAction.ack:
                var ack = new Ack("Test Message");
                logger.Info(Serial.Encode(ack));
                break;
            case FlowCommandAction.daemon:
                new SocketDaemon().Start();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}