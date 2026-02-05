using System;

namespace GENE.Basic.Nodes.SmartThings {
    public class SmartThingsToken(string token) {
        public string Token { get; set; } = token;
    }
}