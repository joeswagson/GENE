using GENE.Nodes.Payloads;
using GENE.Nodes.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace GENE.Nodes.Primitive {
    public interface INullInNode<R> : INode<NullPayload, R> where R : NodeResponse {
        public R Signal();
        R INode<NullPayload, R>.Signal(NullPayload p) => Signal();
    }
}
