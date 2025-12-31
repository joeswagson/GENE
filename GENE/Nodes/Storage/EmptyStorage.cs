using System;
using System.Collections.Generic;
using System.Text;

namespace GENE.Nodes.Storage {
    public class EmptyStorage : PersistentNodeStorage {
        public static readonly EmptyStorage EMPTY = new();

        public void Serialize(BinaryWriter writer) { }
        public static PersistentNodeStorage Deserialize(BinaryReader reader)
            => EMPTY;

    }
}
