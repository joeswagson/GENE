using GENE.Nodes;
using GENE.Nodes.Primitive;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace GENE.Clusters {
    public class NodeCluster(string? id) {
        public string Id = id ?? Guid.NewGuid().ToString();
        protected NodeCluster(INode[] nodes, string? id = null) : this(id)
        {
            Update(nodes);
        }
        public static NodeCluster Deserialize(byte[] data)
        {
            using var ms = new MemoryStream(data);
            using var reader = new BinaryReader(ms);

            var nodeCount = reader.ReadInt32();
            var nodes = new INode[nodeCount];
            for (int i = 0; i < nodeCount; i++)
                nodes[i] = DeserializeNode();

            return new(nodes);

            INode DeserializeNode()
            {
                var nodeName = reader.ReadString();
                var nodeTypeName = reader.ReadString();
                var nodeType = Type.GetType(nodeTypeName) ?? throw new Exception($"Node type '{nodeTypeName}' not found.");
                var node = (INode) Activator.CreateInstance(nodeType)!;
                if (node is IStorageNode storageNode)
                    storageNode.LoadData(reader);

                return node;
            }
        }

        public static byte[] Serialize(NodeCluster cluster)
        {
            using var nodeMs = new MemoryStream();
            using var nodeWriter = new BinaryWriter(nodeMs);
            byte[] SerializeNode(INode node)
            {
                nodeMs.SetLength(0);
                nodeMs.Position = 0;

                var nodeType = node.GetType();
                if (nodeType.AssemblyQualifiedName == null)
                    return [];

                nodeWriter.Write(node.Name); // node name
                nodeWriter.Write(nodeType.AssemblyQualifiedName); // node type name
                if (node is IStorageNode storageNode) // stored data (if any)
                    storageNode.SaveData().Serialize(nodeWriter);

                return nodeMs.ToArray();
            }

            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms);
            INode[] nodes = cluster.Nodes;

            // Node count
            writer.Write(nodes.Length);

            // Serialize each node
            foreach (var node in nodes)
                writer.Write(SerializeNode(node));

            return ms.ToArray();
        }


        public INode[] Nodes = [];

        protected void Update(INode[] nodes)
        {
            Shutdown();
            Nodes = nodes;
            Initialize();
        }
        public INode GetNode(string name) => GetNode<INode>(name);
        public T GetNode<T>(string name) => GetNodes<T>(name).First();
        public IEnumerable<INode> GetNodes(string name) => GetNodes<INode>(name);
        public IEnumerable<T> GetNodes<T>(string name) => Nodes.Where(n => n.Name == name).Cast<T>();
        public void Shutdown()
        {
            foreach (var node in Nodes)
                node.Shutdown();
        }

        protected void Initialize()
        {
            foreach (var node in Nodes)
                try
                {
                    node.Initialize();
                }
                catch
                {
                    
                }
        }
    }
}
