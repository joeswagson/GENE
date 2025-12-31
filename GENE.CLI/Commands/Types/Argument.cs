using System;

namespace GENE.CLI.Commands.Types {
    public class Argument(string name, bool required, string? defaultValue = null) {
        public string Name { get; private set; } = name;
        public bool Required { get; private set; } = required;
        public string? DefaultValue { get; private set; } = defaultValue;

        public override string ToString()
        {
            string inlineValue = DefaultValue == null ? Name : $"{Name}={DefaultValue}";
            return Required ? $"<{inlineValue}>" : $"[{inlineValue}]";
        }
    }
}
