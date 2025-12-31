using GENE.Basic.Nodes.SmartThings;
using GENE.Nodes;
using GENE.Nodes.Primitive;
using GENE.Nodes.Status;
using System;
using System.Collections.Generic;
using System.Text;

namespace GENE.Basic.Nodes.Spotify {
    public enum SpotifyActionType
    {
        Play,
        Pause,
        NextTrack,
        PreviousTrack,
        SetVolume,
        SeekPosition
    }
    public class SpotifyAction : NodePayload
    {

    }
    public class SpotifyStorage : PersistentNodeStorage {
        public static PersistentNodeStorage Deserialize(BinaryReader reader)
        {
            return new SpotifyStorage();
        }

        public void Serialize(BinaryWriter writer)
        {

        }
    }
    public class SpotifyController : IStorageNode<SpotifyStorage, SpotifyAction, WebResponse> {
        public string Name => "Spotify";

        public SpotifyStorage Storage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public static void Create(SpotifyStorage data)
        {
            throw new NotImplementedException();
        }

        public void LoadData(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public WebResponse Signal(SpotifyAction p)
        {
            throw new NotImplementedException();
        }
    }
}
