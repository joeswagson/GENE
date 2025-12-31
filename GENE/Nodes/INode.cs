using GENE.Nodes.Status;
using GENE.Nodes.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace GENE.Nodes {
    public abstract class NodeResponse;
    public abstract class NodePayload;
    public abstract class NodeStatus {
        public abstract void Serialize(BinaryWriter writer);
        public abstract NodeStatus Deserialize(BinaryReader reader);
    }
    public interface PersistentNodeStorage {
        public abstract void Serialize(BinaryWriter writer);
        public static abstract PersistentNodeStorage Deserialize(BinaryReader reader);
    }
    public interface INode {
        public string Name { get; }
        public NodeStatus GetStatus() => EmptyStatus.EMPTY;

        public virtual void Initialize() { }
        public virtual void Shutdown() { }
    }
    public interface INode<P, R> : INode
        where P : NodePayload
        where R : NodeResponse {
        public R Signal(P p);
    }
}
