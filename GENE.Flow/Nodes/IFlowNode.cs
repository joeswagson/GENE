using GENE.Nodes;

namespace GENE.Flow.Nodes;

public record IFlowNode(int Index, string NodeId, INode Node)
{
    public int[] Connections = [];
    public void UpdateConnections(int[] connections) => Connections = connections;
}

public record IFlowNode<P, R>(int Index, string NodeId, INode<P, R> Source) 
    : IFlowNode(Index, NodeId, Source)
    where P : NodePayload
    where R : NodeResponse
{
    public INode<P, R> Source { get; init; } = Source;
}