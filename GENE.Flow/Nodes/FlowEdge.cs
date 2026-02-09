namespace GENE.Flow.Nodes;

public enum EdgeDirection { NONE, IN, OUT }
public struct FlowEdge(FlowNode from, FlowNode to)
{
    public FlowNode Source { get; set; } = from;
    public FlowNode Target { get; set; } = to;

    public EdgeDirection GetDirection(FlowNode reference)
    {
        if (Source == reference) return EdgeDirection.OUT;
        return Target == reference ? EdgeDirection.IN : EdgeDirection.NONE;
    }
}