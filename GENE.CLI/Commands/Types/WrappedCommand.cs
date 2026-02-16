using GENE.CLI.Commands.Types;

namespace GENE.CLI.Commands;

public abstract class WrappedCommand : Command
{
    public override string Identifier => "\0wrapped";
    public override Usage Help() => null!;
}