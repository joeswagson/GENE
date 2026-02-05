namespace GENE.Flow.Nodes;

public enum EdgeDirection { NONE, IN, OUT }
public struct FlowEdge
{
    public FlowNode Source { get; init; }
    public FlowNode Target { get; init; }

    public EdgeDirection GetDirection(FlowNode reference)
    {
        if (Source == reference) return EdgeDirection.OUT;
        if (Target == reference) return EdgeDirection.IN;
        
        return EdgeDirection.NONE;
    }
}