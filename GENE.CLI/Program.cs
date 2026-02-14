global using static GENE.CLI.Program;
using GENE.Basic.Nodes;
using GENE.Basic.Nodes.SmartThings;
using GENE.CLI.Commands.Types;
using GENE.Clusters;
using GENE.JoeRoom;
using JLogger;
using System.Text;
using Color = System.Drawing.Color;

namespace GENE.CLI
{
    public static class Meta
    {
        public const string Version = "1.0.0";
        public const bool Debug = true;
    }

    public class Program
    {
        private static CancellationTokenSource AppSource = new();

        internal static string[] ParseArgs(string input)
        {
            var args = new List<string>();
            var current = new StringBuilder();

            var inQuotes = false;
            var escape = false;

            foreach (var c in input)
            {
                if (escape)
                {
                    current.Append(c);
                    escape = false;
                    continue;
                }

                switch (c)
                {
                    case '\\':
                        escape = true;
                        continue;
                    case '"':
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


        public static readonly Logger Logger = new("gene", Color.CornflowerBlue, debug: Meta.Debug);
        public static NodeCluster? CurrentCluster;

        private static object HandleCommand(string identifier, string[] args)
        {
            var commandObject = CommandRegistry.GetCommand(identifier);
            if (commandObject != null)
            {
                if (args is not ["?"])
                    return commandObject.ExecuteInternal(args);

                Logger.Info(commandObject.Help().ToString());
                return 0;
            }

            Logger.Error("Command", identifier, "not found.");
            return 1;
        }

        private static async Task CommandLoop()
        {
            while (!AppSource.IsCancellationRequested)
            {
                try
                {
                    Logger.Info(newline: false, "cmd>");
                    var input = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(input))
                        continue;

                    var parts = ParseArgs(input);
                    var command = parts[0];
                    var cArgs = parts.Skip(1).ToArray();

                    HandleCommand(command, cArgs);
                }
                catch (Exception e)
                {
                    Logger.Error($"Error executing command: {e.Message}");
                    Logger.Debug(e.StackTrace ?? "");
                }
            }
        }

        private static async Task Main(string[] args)
        {
            if (args.Length > 0)
            {
                CommandRegistry.RegisterCommands();
            }

            Logger.Info("GENE CLI");
            Logger.Info("- General Environment Node Engine");
            Logger.Info("- GENE Version:",     Gene.Meta.Version);
            Logger.Info("- GENE CLI Version:", Meta.Version);
            Logger.Newline();

            CommandRegistry.RegisterCommands();
            _ = Task.Run(CommandLoop);
            await Task.Delay(Timeout.Infinite);
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