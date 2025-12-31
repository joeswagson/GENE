using System;
using System.Collections.Generic;
using System.Text;

namespace GENE.Basic.Nodes.SmartThings {
    public readonly struct SpotifyToken(string token) {
        public readonly string Token { get; } = token;
    }
}
