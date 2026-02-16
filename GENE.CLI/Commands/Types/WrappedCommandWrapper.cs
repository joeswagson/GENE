namespace GENE.CLI.Commands.Types;

public abstract class WrappedCommandWrapper<T> : CommandWrapper<T>
{
    public override string Identifier => "\0wrapped";
}