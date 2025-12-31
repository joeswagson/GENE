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
        public Usage(string description, string command, params Argument[] arguments)
        {
            Description = description;
            Command = command;
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
                sb.Append(Description);
                sb.Append(" | Usage: ");
                sb.Append(Command);
                if (Arguments != null)
                {
                    foreach (Argument arg in Arguments)
                    {
                        sb.Append(" ");
                        sb.Append(arg.ToString());
                    }
                }
                return sb.ToString();
            }
        }
    }
}
