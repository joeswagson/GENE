using GENE.Clusters;
using GENE.Flow.Nodes;
using GENE.Nodes;

namespace GENE.Flow.Clusters;

public class FlowCluster(FlowNode[] nodes) : NodeCluster(nodes.Select(node => node.Node).ToArray())
{
    
}