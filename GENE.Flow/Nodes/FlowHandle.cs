namespace GENE.Flow.Nodes;

/// <summary>
/// Represents a ReactFlow edge handle.
/// </summary>
public struct FlowHandle
{
    public FlowNode Node;
    public FlowEdge[] Connected;
}