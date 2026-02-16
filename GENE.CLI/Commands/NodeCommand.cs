using System.Reflection;
using GENE.Basic.Nodes.SmartThings;
using GENE.CLI.Commands.Types;
using GENE.Clusters;
using GENE.Flow.Typescript;
using GENE.Nodes;

namespace GENE.CLI.Commands;

public enum NodeCommandType
{
    list,
    signal,
    open,
    drop,
}
public class NodeCommand : Command
{
    public override string Identifier => "node";
    public override Usage Help()
    {
        return new(Identifier, "Provides an interface to interact with nodes.");
    }

    private readonly Dictionary<string, NodeCluster> _clusters = [];
    protected static string[] SkipFirst(string[] src) => src.Skip(1).ToArray();
    public override object Execute(string[] args) => Required(0, Enum.Parse<NodeCommandType>) switch {
        NodeCommandType.list => ListCommand(),
        NodeCommandType.signal => SignalCommand(),
        NodeCommandType.open => OpenClusterCommand(),
        NodeCommandType.drop => DropClusterCommand(),
        _ => throw new ArgumentOutOfRangeException(nameof(args))
    };

    public object ListCommand()
    {
        var selector = Required<string>(1);
        switch (selector)
        {
            case "clusters": {
                foreach (var cluster in _clusters)
                    Logger.Info($"Cluster: {cluster.Value.GetType().Name} ({cluster.Value.Nodes.Length}) {cluster.Key}");
                return 0;
            }
            
            case "nodes": {
                var clusterId = Required<string>(2);
                if (!_clusters.TryGetValue(clusterId, out var match))
                    return new ArgumentException($"{clusterId} could not be found.");

                foreach (var node in match.Nodes)
                    Logger.Info($"Node: {node.Name} ({node.GetType().Name}, {node.GetType().Assembly.GetName().Name})");
                return 0;
            }
        }

        var asmName = Required<string>(2);
        var className = $"{Assembly.Load(asmName).GetTypes().First(t=>t.Name.Contains(selector))}, {asmName}, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        
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

    public object SignalCommand()
    {
        var typeName = Required<string>(1);
        var asmName = Required<string>(2);
        var className = $"{Assembly.Load(asmName).GetTypes().First(t=>t.Name.Contains(typeName))}, {asmName}, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        
        var type = Type.GetType(className);
        if (type is null || type.IsAssignableFrom(typeof(INode)))
            return new ArgumentException($"{className} could not be found or does not implement {nameof(INode)}.");

        var typistNode = Typist.Convert(type);
        var signal = Argument(3, "Signal");
        // if(typistNode.Root.Signal.Any(m=>m.Name == signal))
                   
        
        return 0;
    }

    public object OpenClusterCommand()
    {
        var typeName = Required<string>(1);
        var asmName = Required<string>(2);
        var className = $"{Assembly.Load(asmName).GetTypes().First(t=>t.Name.Contains(typeName))}, {asmName}, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        
        var type = Type.GetType(className);
        if (type is null || type.IsAssignableFrom(typeof(NodeCluster)))
            return new ArgumentException($"{className} could not be found or does not implement {nameof(NodeCluster)}.");

        if (Activator.CreateInstance(type) is not NodeCluster cluster)
            return new ArgumentException($"{className} could not be instantiated.");
        
        _clusters[cluster.Id] = cluster;
        Logger.Info($"Opened Cluster: {cluster.Id}");
        
        return 0;
    }

    public object DropClusterCommand()
    {
        var clusterId =  Required<string>(1);
        if (!_clusters.TryGetValue(clusterId, out var match))
            return new ArgumentException($"{clusterId} could not be found.");
        
        Logger.Info($"Shutting down cluster...");
        match.Shutdown();
        _clusters.Remove(clusterId);
        Logger.Info($"Dropped Cluster: {match.Id}");
        return 0;
    }
}