using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GENE.CLI.Commands.Types
{
    public class CommandRegistry
    {
        private static Dictionary<string, Command> Commands = new Dictionary<string, Command>();

        public static void RegisterCommand(Command command)
        {
            Commands.Add(command.Identifier, command);
            Logger.Debug("Registered command", command.Identifier);
        }

        public static Command? GetCommand(string command)
        {
            if (Commands.ContainsKey(command))
            {
                return Commands[command];
            }
            return null;
        }

        public static Command[] GetCommands()
        {
            return Commands.Values.ToArray();
        }

        public static bool RegisterCommands()
        {
            try
            {
                RegisterCommand(new HelpCommand());
                RegisterCommand(new ExitCommand());
                RegisterCommand(new SmartCommand());
                RegisterCommand(new TypistCommand());
                RegisterCommand(new FlowCommand());
                RegisterCommand(new NodeCommand());
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
