using GENE.Nodes;
using GENE.Nodes.Payloads;
using GENE.Nodes.Primitive;
using GENE.Nodes.Responses;
using GENE.Nodes.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GENE.Basic.Nodes {
    public class DebugNode : ITriggerNode {
        public string Name => "Debug";
        public void Signal()
        {
            Console.WriteLine("Triggered!");
        }
    }
}
