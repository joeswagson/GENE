using System;
using System.Collections.Generic;
using System.Text;

namespace GENE.Basic.Nodes.SmartThings {
    public interface ISmartSwitchable {
        void On() => Toggle(true);
        void Off() => Toggle(false);
        void Toggle(bool enabled);
    }
    public interface ISmartDimmable : ISmartSwitchable {
        void SetLevel(byte level);
    }
    public interface ISmartAudio : ISmartSwitchable {
        void SetVolume(byte level);
        void VolumeUp();
        void VolumeDown();
        void Mute();
        void Unmute();
    }
}
