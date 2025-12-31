using GENE.Basic.Nodes.SmartThings;
using GENE.Clusters;
using System.IO;

namespace GENE.JoeRoom {
    public class RoomCluster : NodeCluster {
        private readonly SmartThingsToken Auth;
        public SmartThingsBulb Ceiling;
        public SmartThingsTV TV;

        public RoomCluster(string smartToken) : base([])
        {
            Auth = new SmartThingsToken(smartToken);

            Ceiling = new(Auth, "ceiling", "ade2edfb-cec5-4046-bed5-6eb868a18020");
            TV = new(Auth, "tv", "c8540310-3292-ce94-f914-38ebe1c91cc4");

            Update([Ceiling, TV]);
        }
    }
}
