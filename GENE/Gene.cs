using System.Drawing;
using JLogger;

namespace GENE
{
    public static class Gene {
        public static class Meta {
            public const string Version = "1.0.0";
        }

        public static Logger Logger = new("gene", Color.CornflowerBlue);
    }
}
