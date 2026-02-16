using System.Net.Http.Headers;
using GENE.Flow.Clusters;
using GENE.Nodes;

namespace GENE.Flow.Nodes;

public enum SignalType
{
    Signal, Output
}
public record FlowNode(FlowCluster Cluster, int Index, string NodeId, INode Node)
{
    public List<FlowHandle> Signals = [];
    public List<FlowHandle> Outputs = [];

    public void AddHandle(SignalType type, FlowHandle handle)
    {

        switch (type)
        {
            case SignalType.Signal:
                Signals.Add(handle);
                break;
            case SignalType.Output:
                Outputs.Add(handle);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public void Signalled(SignalActivation signal)
    {
        
    }

    public void Propagate()
    {
        
    }
}

// public record FlowNode<TP, TR>(FlowCluster Cluster, int Index, string NodeId, INode<TP, TR> Source) 
//     : FlowNode(Cluster, Index, NodeId, Source)
//     where TP : NodePayload
//     where TR : NodeResponse;