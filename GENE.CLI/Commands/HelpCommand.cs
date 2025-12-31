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
        public override void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                foreach (Command command in CommandRegistry.GetCommands())
                {
                    logger.Info(command.Help().ToString());
                }
            }
            else
            {
                var command = CommandRegistry.GetCommand(args[0]);
                if (command != null)
                {
                    logger.Info(command.Help().ToString());
                }
                else
                {
                    logger.Info("Command not found");
                }
            }
        }
    }
}
