using GENE.Flow.Clusters;
using GENE.Nodes;

namespace GENE.Flow.Nodes;

public record FlowNode(FlowCluster Cluster, int Index, string NodeId, INode Node)
{
    public List<FlowEdge> Signals = [];
    public List<FlowEdge> Outputs = [];

    public void Signalled(SignalActivation signal)
    {
        
    }

    public void Propagate()
    {
        
    }
}

public record FlowNode<TP, TR>(FlowCluster Cluster, int Index, string NodeId, INode<TP, TR> Source) 
    : FlowNode(Cluster, Index, NodeId, Source)
    where TP : NodePayload
    where TR : NodeResponse;