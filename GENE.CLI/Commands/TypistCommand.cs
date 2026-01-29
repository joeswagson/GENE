using System.Reflection;
using GENE.Basic.Nodes.SmartThings;
using GENE.CLI.Commands.Types;
using GENE.Flow.Typescript;
using GENE.Flow.Typescript.Members;
using GENE.Nodes;

namespace GENE.CLI.Commands;

public class TypistCommand : Command
{
    public override string Identifier => "typist";

    public override Usage Help()
    {
        return new Usage(
            Identifier,
            "Typist reflection interface.",
            new Argument("class", true, nameof(SmartThingsDevice)));
    }

    public class Sample
    {
    }

    public override void Execute(string[] args)
    {
        var className = Argument<string>(0, typeof(SmartThingsDevice).AssemblyQualifiedName);
        var type = Type.GetType(className);
        if (type is null || type.IsAssignableFrom(typeof(INode)))
            throw new ArgumentException($"{className} could not be found or does not implement {nameof(INode)}.");

        var typistNode = Typist.Convert(type, typeof(SmartThingsAction), typeof(WebResponse));
        
        logger.InfoSplit(typistNode.Root.ToString());
        logger.InfoSplit(typistNode.Payload?.ToString() ?? "no payload");
        logger.InfoSplit(typistNode.Response?.ToString() ?? "no response");
    }
}