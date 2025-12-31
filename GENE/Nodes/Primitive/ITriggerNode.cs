using GENE.Nodes.Payloads;
using GENE.Nodes.Responses;

namespace GENE.Nodes.Primitive {
    public interface ITriggerNode : INode<NullPayload, NullResponse> {
        public void Signal();
        NullResponse INode<NullPayload, NullResponse>.Signal(NullPayload p)
        {
            Signal();
            return NullResponse.NULL;
        }
    }
}
