using GENE.Nodes.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GENE.Nodes.Primitive {
    public interface IStorageNode {
        void LoadData(BinaryReader reader);
        public PersistentNodeStorage SaveData() => EmptyStorage.EMPTY;
    }
    public interface IStorageNode<T> : IStorageNode, INode
        where T : PersistentNodeStorage {
        public T Storage { get; set; }
        public static abstract void Create(T data);
    }

    public interface IStorageNode<T, P, R> : IStorageNode<T>, INode<P, R>
        where T : PersistentNodeStorage
        where P : NodePayload
        where R : NodeResponse;
    public interface IStorageOutNode<T, R> : IStorageNode<T>, INullInNode<R>
        where T : PersistentNodeStorage
        where R : NodeResponse;
    public interface IStorageInNode<T, P> : IStorageNode<T>, INullOutNode<P>
        where T : PersistentNodeStorage
        where P : NodePayload;
}
