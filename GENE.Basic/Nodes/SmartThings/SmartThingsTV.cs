using GENE.Nodes;

namespace GENE.Basic.Nodes.SmartThings {
    public class SmartThingsTV(SmartThingsToken token, string name, string id)
        : SmartThingsDevice(name, id), ISmartAudio {
        private SmartThingsToken Auth = token;
        public void UpdateToken(SmartThingsToken token) => Auth = token;
        private void Send(string capability, string command, params object[] args)
        {
            Signal(
                new(Auth.Token,
                    new SmartThingsCommand {
                        component = "main",
                        capability = capability,
                        command = command,
                        arguments = args.Length > 0 ? args : null
                    }
                )
            );
        }

        private void SendKey(string key)
        {
            Send("samsungvd.remoteControl", "send", key);
        }
        public void Toggle(bool on)
        {
            Send("switch", on ? "on" : "off");
        }
        public void SetVolume(byte level)
        {
            Send("audioVolume", "setVolume", level);
        }

        public void VolumeUp()
        {
            Send("audioVolume", "volumeUp");
        }

        public void VolumeDown()
        {
            Send("audioVolume", "volumeDown");
        }

        public void Mute()
        {
            Send("audioMute", "mute");
        }

        public void Unmute()
        {
            Send("audioMute", "unmute");
        }
        public void ChangeSource(string source)
        {
            Send("mediaInputSource", "setInputSource", source);
        }

        public void Home() => SendKey("HOME");
        public void Back() => SendKey("BACK");
        public void Up() => SendKey("UP");
        public void Down() => SendKey("DOWN");
        public void Left() => SendKey("LEFT");
        public void Right() => SendKey("RIGHT");
        public void Enter() => SendKey("ENTER");
        public void Exit() => SendKey("EXIT");
        public void Menu() => SendKey("MENU");
        public void Play()
        {
            Send("mediaPlayback", "play");
        }
        public void Pause()
        {
            Send("mediaPlayback", "pause");
        }
        public void Stop()
        {
            Send("mediaPlayback", "stop");
        }
        public void FastForward()
        {
            Send("mediaPlayback", "fastForward");
        }
        public void Rewind()
        {
            Send("mediaPlayback", "rewind");
        }
        public void ChannelUp()
        {
            Send("tvChannel", "channelUp");
        }

        public void ChannelDown()
        {
            Send("tvChannel", "channelDown");
        }

        public void SetChannel(int channel)
        {
            Send("tvChannel", "setChannel", channel);
        }

        public void SetPictureMode(string mode)
        {
            Send("custom.picturemode", "setPictureMode", mode);
        }

        public void SetSoundMode(string mode)
        {
            Send("custom.soundmode", "setSoundMode", mode);
        }

        public void LaunchApp(string appId)
        {
            Send("custom.launchapp", "launchApp", appId);
        }

        public void AmbientOn()
        {
            Send("samsungvd.ambient", "on");
        }

        public void AmbientOff()
        {
            Send("samsungvd.ambient", "off");
        }
        public void Refresh()
        {
            Send("refresh", "refresh");
        }
    }
}
