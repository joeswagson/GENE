using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GENE.CLI.Commands.Types {
    public class Usage
    {
        public string? Description { get; private set; }
        public string? Command { get; private set; }
        public List<Argument>? Arguments { get; private set; }

        private string? overwriteUsage;
        public Usage(string command, string description, params Argument[] arguments)
        {
            Command = command;
            Description = description;
            Arguments = arguments.ToList();
        }
        public Usage(string customUsage)
        {
            overwriteUsage = customUsage;
        }

        public override string ToString()
        {
            if (overwriteUsage != null)
            {
                return overwriteUsage;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Command);
                sb.Append(" - ");
                sb.Append(Description);
                sb.Append(" | Usage: ");
                sb.Append(Command);
                if (Arguments != null)
                {
                    foreach (var arg in Arguments)
                    {
                        sb.Append(' ');
                        sb.Append(arg.ToString());
                    }
                }
                return sb.ToString();
            }
        }
    }
}
