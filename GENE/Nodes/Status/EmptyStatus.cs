using System;
using System.Collections.Generic;
using System.Text;

namespace GENE.Nodes.Status {
    public class EmptyStatus : NodeStatus {
        public static readonly EmptyStatus EMPTY = new();

        public override void Serialize(BinaryWriter writer) { }
        public override NodeStatus Deserialize(BinaryReader reader) => EMPTY;

    }
}
