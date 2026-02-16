using System.Diagnostics;
using GENE.Basic.Nodes.SmartThings;
using GENE.Clusters;
using System.IO;
using GENE.Basic.Nodes.Custom;
using GENE.Nodes;

namespace GENE.JoeRoom
{
    public class RoomCluster : NodeCluster
    {
        public RoomCluster(string? id = null) : base(id)
        {
            List<INode> nodes = [
                Ceiling,
                Tv
            ];
            
            AddNode(nodes, KeyboardRemote);
            Update(nodes.ToArray());

            KeyboardRemote?.ButtonPressed += (button) =>
            {
                Debug.WriteLine("Button Pressed: {0}, {1}:", button.X, button.Y);
            };
        } 
        
        #region OAuth
        
        private static readonly OAuthManager SmartThings = new(
            AppConfig.Secrets["PKEYS:SMARTTHINGS_CLIENT_ID"]!,
            AppConfig.Secrets["PKEYS:SMARTTHINGS_CLIENT_SECRET"]!,
            AppConfig.Secrets["PKEYS:SMARTTHINGS_REDIRECT_HTTP"]!);
        
        #endregion

        #region Node Definitions

        public readonly SmartThingsBulb Ceiling = new(SmartThings, "ceiling", "ade2edfb-cec5-4046-bed5-6eb868a18020");
        public readonly SmartThingsTv Tv = new(SmartThings, "tv", "");

        public readonly IrRemote KeyboardRemote =
            new("/dev/ttyACM0", new IrButtonMapping(new() {
                { "On",        new(0, 0) },
                { "Button1",   new(1, 0) },
                { "Off",       new(2, 0) },
                { "Bright+",   new(0, 1) },
                { "M+",        new(1, 1) },
                { "Bright-",   new(2, 1) },
                { "S+",        new(0, 2) },
                { "Auto",      new(1, 2) },
                { "S-",        new(2, 2) },
                { "Magenta",   new(0, 3) },
                { "M-",        new(1, 3) },
                { "White",     new(2, 3) },
                { "Red",       new(0, 4) },
                { "Green",     new(1, 4) },
                { "Blue",      new(2, 4) },
                { "Yellow",    new(0, 5) },
                { "Sunflower", new(1, 5) },
                { "Orange",    new(2, 5) },
                { "Emerald",   new(0, 6) },
                { "Sky",       new(1, 6) },
                { "Marine",    new(2, 6) },
            }));

        #endregion

        #region Helpers

        private static T? TryNode<T>(Func<T> getter) where T : class, INode
        {
            try
            {
                return getter();
            }
            catch
            {
                return null;
            }
        }

        private static void AddNode(List<INode> collection, INode? node)
        {
            if (node is null)
                return;
            
            collection.Add(node);
        }
        

        #endregion
    }
}