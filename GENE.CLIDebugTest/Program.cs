using GENE.Basic.Nodes;

namespace GENE.CLIDebugTest {
    internal class Program {
        static void Main(string[] args)
        {
            DebugNode debugNode = new DebugNode();
            debugNode.Signal();
        }
    }
}
