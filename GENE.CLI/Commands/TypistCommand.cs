using System.Reflection;
using GENE.Basic.Nodes.SmartThings;
using GENE.CLI.Commands.Types;
using GENE.Flow.Typescript;
using GENE.Flow.Typescript.Members;
using GENE.Nodes;

namespace GENE.CLI.Commands;

public class TypistCommand : Command
{
    public static readonly Type Default = typeof(SmartThingsBulb);
    public override string Identifier => "typist";

    public override Usage Help()
    {
        return new Usage(
            Identifier,
            "Typist reflection interface.",
            new Argument("class", true, Default.AssemblyQualifiedName));
    }

    public class Sample
    {
    }

    public override object Execute(string[] args)
    {
        var className = Argument(0, Default.AssemblyQualifiedName);
        var quickAssembly = Argument(1, string.Empty);
        if(quickAssembly != string.Empty)
            className = $"{Assembly.Load(quickAssembly).GetTypes().First(t=>t.Name.Contains(className))}, {quickAssembly}, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        
        var type = Type.GetType(className);
        if (type is null || type.IsAssignableFrom(typeof(INode)))
            throw new ArgumentException($"{className} could not be found or does not implement {nameof(INode)}.");

        var typistNode = Typist.Convert(type, typeof(SmartThingsAction), typeof(WebResponse));
        
        Logger.InfoSplit(typistNode.Root.ToString());
        Logger.InfoSplit(typistNode.Payload?.ToString() ?? "no payload");
        Logger.InfoSplit(typistNode.Response?.ToString() ?? "no response");

        return 0;
    }
}