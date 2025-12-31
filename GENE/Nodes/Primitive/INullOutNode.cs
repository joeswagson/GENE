using GENE.Nodes.Payloads;
using GENE.Nodes.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace GENE.Nodes.Primitive {
    public interface INullOutNode<P> : INode<P, NullResponse> where P : NodePayload {
        public new void Signal(P p);
        NullResponse INode<P, NullResponse>.Signal(P p)
        {
            Signal(p);
            return NullResponse.NULL;
        }
    }
}