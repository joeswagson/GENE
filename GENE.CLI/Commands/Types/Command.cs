using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GENE.CLI.Commands.Types {
    public abstract class Command
    {
        public abstract string Identifier { get; }
        public abstract Usage Help();
        public abstract void Execute(string[] args);
    }
}
