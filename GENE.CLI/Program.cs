global using static GENE.CLI.Program;
using GENE.Basic.Nodes;
using GENE.Basic.Nodes.SmartThings;
using GENE.CLI.Commands.Types;
using GENE.Clusters;
using GENE.JoeRoom;
using JLogger;
using System.Drawing;
using System.Text;

namespace GENE.CLI {
    public static class Meta {
        public const string VERSION = "1.0.0";
    }
    public class Program {
        static string[] ParseArgs(string input)
        {
            var args = new List<string>();
            var current = new StringBuilder();

            bool inQuotes = false;
            bool escape = false;

            foreach (char c in input)
            {
                if (escape)
                {
                    current.Append(c);
                    escape = false;
                    continue;
                }

                if (c == '\\')
                {
                    escape = true;
                    continue;
                }

                if (c == '"')
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (char.IsWhiteSpace(c) && !inQuotes)
                {
                    if (current.Length > 0)
                    {
                        args.Add(current.ToString());
                        current.Clear();
                    }
                    continue;
                }

                current.Append(c);
            }

            if (current.Length > 0)
                args.Add(current.ToString());

            return args.ToArray();
        }


        public static readonly Logger logger = new("gene", Color.CornflowerBlue);
        public static NodeCluster? CurrentCluster;
        static void Main(string[] args)
        {
            logger.Info("GENE CLI");
            logger.Info("- General Environment Node Engine");
            logger.Info("- GENE Version:", Gene.Meta.VERSION);
            logger.Info("- GENE CLI Version:", Meta.VERSION);
            logger.Newline();

            CommandRegistry.RegisterCommands();

            string pat = args[0];
            logger.Info("PAT:", pat);

            CurrentCluster = new RoomCluster(pat);

            while (true)
            {
                logger.LogNewline(Logger.LogStyle.Info, false, "cmd>");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                var parts = ParseArgs(input);
                var command = parts[0];
                var cArgs = new string[parts.Length - 1];
                Array.Copy(parts, 1, cArgs, 0, cArgs.Length);

                try
                {
                    var commandObject = CommandRegistry.GetCommand(command);
                    if (commandObject != null)
                        if (cArgs.Length == 1 && cArgs[0] == "?")
                            logger.Info(commandObject.Help().ToString());
                        else
                            commandObject.Execute(cArgs);
                    else
                        logger.Error("Command", command, "not found.");
                } catch (Exception e)
                {
                    logger.Error($"An error occured while executing command \"{command}\")");
                    logger.Error($"- Error: {e.Message}");
                    logger.Error($"- Arguments:", cArgs);
                }
            }
            //var action = SmartActions.Light(light.Name, pat);

            //logger.Info("Signalling...");
            //logger.Info("Response Code:", light.Signal(action).StatusCode);
            //logger.Info("Signalled.");

            //var node = new DebugNode();
            //var cluster = new NodeCluster([node]);

            //logger.Info("Signalling...");
            //node.Signal();
            //logger.Info("Signalled.");
            //logger.Newline();

            //byte[] serialized = NodeCluster.Serialize(cluster);
            //var deserialized = NodeCluster.Deserialize(serialized);
            //var deserializedNode = (DebugNode) deserialized.Nodes[0];

            //logger.Info("Signalling...");
            //deserializedNode.Signal();
            //logger.Info("Signalled.");
        }
    }
}
