using System.Reflection;
using GENE.Basic.Nodes.SmartThings;
using GENE.CLI.Commands.Types;
using GENE.Clusters;
using GENE.Flow.Typescript;
using GENE.Nodes;

namespace GENE.CLI.Commands;

public class NodeCommand : CommandWrapper<string>
{
    public override string Identifier => "node";

    public override Dictionary<string, Command> Routes { get; } = new() {
        { "list", new ListCommand() },
        { "signal", new SignalCommand() },
        { "open", new OpenClusterCommand() },
        { "drop", new DropClusterCommand() }
    };

    public override Usage Help()
    {
        return new(Identifier, "Provides an interface to interact with nodes.");
    }
    // protected static string[] SkipFirst(string[] src) => src.Skip(1).ToArray();
}

public class ListCommand : WrappedCommand
{
    public override object Execute(string[] args)
    {
        var selector = Required<string>(0);
        switch (selector)
        {
            case "clusters": {
                foreach (var cluster in Global.Clusters)
                    Logger.Info(
                        $"Cluster: {cluster.Value.GetType().Name} ({cluster.Value.Nodes.Length}) {cluster.Key}");
                return 0;
            }

            case "nodes": {
                var clusterId = Required<string>(1);
                if (!Global.Clusters.TryGetValue(clusterId, out var match))
                    return new ArgumentException($"{clusterId} could not be found.");

                foreach (var node in match.Nodes)
                    Logger.Info($"Node: {node.Name} ({node.GetType().Name}, {node.GetType().Assembly.GetName().Name})");
                return 0;
            }
        }

        var asmName = Required<string>(1);
        var className =
            $"{Assembly.Load(asmName).GetTypes().First(t => t.Name.Contains(selector))}, {asmName}, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

        var type = Type.GetType(className);
        if (type is null || type.IsAssignableFrom(typeof(INode)))
            return new ArgumentException($"{className} could not be found or does not implement {nameof(INode)}.");

        var typistNode = Typist.Convert(type);
        foreach (var signal in typistNode.Root.Signal)
            Logger.Info($"Signal: {signal.Name}: {signal.Type}");
        foreach (var output in typistNode.Root.Output)
            Logger.Info($"Output: {output.Name}: {output.Type}");

        return 0;
    }
}
public class SignalCommand : WrappedCommand
{
    public override object Execute(string[] args)
    {
        var typeName = Required<string>(0);
        var asmName = Required<string>(1);
        var className =
            $"{Assembly.Load(asmName).GetTypes().First(t => t.Name.Contains(typeName))}, {asmName}, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

        var type = Type.GetType(className);
        if (type is null || type.IsAssignableFrom(typeof(INode)))
            return new ArgumentException($"{className} could not be found or does not implement {nameof(INode)}.");

        var typistNode = Typist.Convert(type);
        var signal = Argument(3, "Signal");
        // if(typistNode.Root.Signal.Any(m=>m.Name == signal))


        return 0;
    }
}
public class OpenClusterCommand : WrappedCommand
{
    public override object Execute(string[] args)
    {
        var typeName = Required<string>(0);
        var asmName = Required<string>(1);
        var className =
            $"{Assembly.Load(asmName).GetTypes().First(t => t.Name.Contains(typeName))}, {asmName}, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

        var type = Type.GetType(className);
        if (type is null || type.IsAssignableFrom(typeof(NodeCluster)))
            return new ArgumentException(
                $"{className} could not be found or does not implement {nameof(NodeCluster)}.");

        if (Activator.CreateInstance(type) is not NodeCluster cluster)
            return new ArgumentException($"{className} could not be instantiated.");

        Global.Clusters[cluster.Id] = cluster;
        Logger.Info($"Opened Cluster: {cluster.Id}");

        return 0;
    }
}
public class DropClusterCommand : WrappedCommand
{
    public override object Execute(string[] args)
    {
        var clusterId = Required<string>(0);
        if (!Global.Clusters.TryGetValue(clusterId, out var match))
            return new ArgumentException($"{clusterId} could not be found.");

        Logger.Info($"Shutting down cluster...");
        match.Shutdown();
        Global.Clusters.Remove(clusterId);
        Logger.Info($"Dropped Cluster: {match.Id}");
        return 0;
    }
}