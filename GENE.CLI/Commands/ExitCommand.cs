using GENE.CLI.Commands.Types;

namespace GENE.CLI.Commands;

public class ExitCommand : Command
{
    public override string Identifier => "exit";
    public override Usage Help() => new(Identifier, "Closes the program.");

    public override void Execute(string[] args)
    {
        Environment.Exit(0);
    }
}