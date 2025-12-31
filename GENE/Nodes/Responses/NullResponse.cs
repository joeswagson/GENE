using System;
using System.Collections.Generic;
using System.Text;

namespace GENE.Nodes.Responses {
    /// <summary>
    /// Represents a response that contains no data or result information.
    /// </summary>
    public class NullResponse : NodeResponse
    {
        public static readonly NullResponse NULL = new();
    }
}
