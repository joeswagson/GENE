using GENE.CLI.Commands.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GENE.CLI.Commands
{
    public class HelpCommand : Command
    {
        public override string Identifier => "help";
        public override Usage Help()
        {
            return new Usage("help", "Shows this help message");
        }
        public override object Execute(string[] args)
        {
            if (args.Length == 0)
                foreach (var command in CommandRegistry.GetCommands())
                    Logger.Info(command.Help().ToString());
            else
            {
                var command = CommandRegistry.GetCommand(args[0]);
                if (command != null)
                    Logger.Info(command.Help().ToString());
                else
                {
                    Logger.Info("Command not found");
                    return 1;
                }
            }
            
            return 0;
        }
    }
}
