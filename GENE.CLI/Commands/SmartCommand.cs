using GENE.Basic.Nodes.SmartThings;
using GENE.CLI.Commands.Types;
using System;

namespace GENE.CLI.Commands {
    public enum SmartAction {
        // common
        power,

        // lights
        level,

        // audio / tv
        volume,
        volumeup,
        volumedown,
        mute,
        unmute,

        // tv navigation
        source,
        home,
        back,
        play,
        pause
    }

    public class SmartCommand : Command {
        public override string Identifier => "smart";

        public override void Execute(string[] args)
        {
            if (CurrentCluster == null)
                throw new InvalidOperationException("No cluster selected.");

            if (args.Length < 2)
                throw new ArgumentException("Usage: smart <device> <action> [args...]");

            var deviceName = args[0];
            var action = Enum.Parse<SmartAction>(args[1], ignoreCase: true);

            var device = CurrentCluster.GetNode<SmartThingsDevice>(deviceName);

            switch (action)
            {
                // ---------- COMMON ----------
                case SmartAction.power:
                {
                    if (device is not ISmartSwitchable switchable)
                        throw new InvalidOperationException("Device does not support switching on and off.");

                    if (args.Length < 3)
                        throw new ArgumentException("Usage: smart <device> power <true|false>");

                    var state = bool.Parse(args[2]);
                    switchable.Toggle(state);
                    break;
                }

                // ---------- LIGHTS ----------
                case SmartAction.level:
                {
                    if (device is not ISmartDimmable dimmable)
                        throw new InvalidOperationException("Device does not support brightness.");

                    if (args.Length < 3)
                        throw new ArgumentException("Usage: smart <device> level <0-100>");

                    dimmable.SetLevel(byte.Parse(args[2]));
                    break;
                }

                // ---------- AUDIO / TV ----------
                case SmartAction.volume:
                {
                    if (device is not ISmartAudio audio)
                        throw new InvalidOperationException("Device does not support volume.");

                    audio.SetVolume(byte.Parse(args[2]));
                    break;
                }

                case SmartAction.volumeup:
                    Require<ISmartAudio>(device).VolumeUp();
                    break;

                case SmartAction.volumedown:
                    Require<ISmartAudio>(device).VolumeDown();
                    break;

                case SmartAction.mute:
                    Require<ISmartAudio>(device).Mute();
                    break;

                case SmartAction.unmute:
                    Require<ISmartAudio>(device).Unmute();
                    break;

                // ---------- TV ONLY ----------
                case SmartAction.source:
                {
                    Require<SmartThingsTV>(device).ChangeSource(args[2]);
                    break;
                }

                case SmartAction.home:
                    Require<SmartThingsTV>(device).Home();
                    break;

                case SmartAction.back:
                    Require<SmartThingsTV>(device).Back();
                    break;

                case SmartAction.play:
                    Require<SmartThingsTV>(device).Play();
                    break;

                case SmartAction.pause:
                    Require<SmartThingsTV>(device).Pause();
                    break;

                default:
                    throw new NotSupportedException($"Unsupported action: {action}");
            }
        }

        private static T Require<T>(SmartThingsDevice device) where T : class
        {
            if (device is not T cast)
                throw new InvalidOperationException(
                    $"Device does not support {typeof(T).Name}"
                );
            return cast;
        }

        public override Usage Help() => new(
            Identifier,
            "Controls SmartThings devices (lights, TVs, audio).",
            new Argument("device", true),
            new Argument("action", true),
            new Argument("args...", false)
        );
    }
}
