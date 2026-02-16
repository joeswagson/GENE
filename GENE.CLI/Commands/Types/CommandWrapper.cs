using GENE.CLI.Commands.Types;

namespace GENE.CLI.Commands;

public abstract class CommandWrapper<T> : Command where T : notnull
{
    public abstract Dictionary<T, Command> Routes { get; }

    public override Usage Help() => new($"Usage: {Identifier} <{string.Join('|', Routes.Keys)}>");

    public override object Execute(string[] args)
    {
        var route = Argument<T>(0);
        var routedArgs = args.Skip(1).ToArray();
        
        if (route is not null && Routes.TryGetValue(route, out var command)) 
            return command.ExecuteInternal(routedArgs);
        
        // route not found
        var keysJoined = string.Join(", ", Routes.Keys);
        return new Exception($"Invalid route. Choices: {keysJoined}");
    }
}