using System.Text;
using System.Drawing;
using Pastel;

namespace JLogger
{
    public class Logger(string tag, Color? tagColor = null, bool debug = false)
    {
        public string Tag { get; private set; } = tag;
        public Color TagColor { get; private set; } = tagColor ?? Color.Gray;
        public bool IsDebug { get; private set; } = debug;

        private static readonly object ConsoleLock = new();

        #region LogStyle Flags

        [Flags]
        public enum LogStyle
        {
            Info = 0,
            Warn = 1 << 0,
            Error = 1 << 1,
            Debug = 1 << 2
        }

        #endregion

        #region Color Math

        public static (int, int, int) Hsl2Rgb(double h, double sl, double l)
        {
            double r = l, g = l, b = l;
            var v = l <= 0.5 ? l * (1.0 + sl) : l + sl - l * sl;
            if (!(v > 0))
                return (
                    Convert.ToByte(Math.Clamp(r * 255.0, 0, 255)),
                    Convert.ToByte(Math.Clamp(g * 255.0, 0, 255)),
                    Convert.ToByte(Math.Clamp(b * 255.0, 0, 255))
                );

            var m = l + l - v;
            var sv = (v - m) / v;
            h *= 6.0;
            var sextant = (int)h;
            var fract = h - sextant;
            var vsf = v * sv * fract;
            var mid1 = m + vsf;
            var mid2 = v - vsf;

            switch (sextant)
            {
                case 0:
                    r = v;
                    g = mid1;
                    b = m;
                    break;
                case 1:
                    r = mid2;
                    g = v;
                    b = m;
                    break;
                case 2:
                    r = m;
                    g = v;
                    b = mid1;
                    break;
                case 3:
                    r = m;
                    g = mid2;
                    b = v;
                    break;
                case 4:
                    r = mid1;
                    g = m;
                    b = v;
                    break;
                case 5:
                    r = v;
                    g = m;
                    b = mid2;
                    break;
            }

            return (
                Convert.ToByte(Math.Clamp(r * 255.0, 0, 255)),
                Convert.ToByte(Math.Clamp(g * 255.0, 0, 255)),
                Convert.ToByte(Math.Clamp(b * 255.0, 0, 255))
            );
        }

        private static Color BrightenColor(Color input, float amount)
        {
            var (r, g, b) = Hsl2Rgb(
                input.GetHue() / 360,
                input.GetSaturation() / amount,
                input.GetBrightness() * amount
            );
            return Color.FromArgb(255, r, g, b);
        }

        private static Color SaturateColor(Color input, float amount)
        {
            var (r, g, b) = Hsl2Rgb(
                input.GetHue() / 360,
                input.GetSaturation() * amount,
                input.GetBrightness()
            );
            return Color.FromArgb(255, r, g, b);
        }

        #endregion

        #region ANSI Tag Colors

        public static Color ErrorTagColor = Color.IndianRed;
        public static Color DebugTagColor = Color.HotPink;
        public static Color WarnTagColor = Color.Gold;

        public static string GetFormattedTag(string tag, Color? colorNullable)
        {
            if (colorNullable is null)
                return $"[{tag}] ";

            var color = colorNullable.Value;
            var leading = "[".Pastel(color);
            var trailing = "]".Pastel(color);
            var inner = tag.Pastel(BrightenColor(SaturateColor(color, 1.5f), 1.2f));

            return $"{leading}{inner}{trailing} ";
        }

        public string GetTagString(bool format = true)
            => GetFormattedTag(Tag, format ? TagColor : null);

        public static string GetErrorTag(bool format = true)
            => GetFormattedTag("ERROR", format ? ErrorTagColor : null);

        public static string GetWarnTag(bool format = true)
            => GetFormattedTag("WARN", format ? WarnTagColor : null);

        public static string GetDebugTag(bool format = true)
            => GetFormattedTag("DEBUG", format ? DebugTagColor : null);

        #endregion

        #region Terminal.Gui TrueColor

        private static Terminal.Gui.Drawing.Color FromSystem(Color color) => new(color.R, color.G, color.B);

        public static void LogFormattedTag(string tag, Color? colorNullable)
        {
            if (colorNullable is null)
            {
                Tui.AppendLogRaw($"[{tag}]");
                return;
            }

            var color = colorNullable.Value;
            var terminal = FromSystem(color);
            System.Diagnostics.Debug.WriteLine("I EXIST!!!");
            Tui.AppendLogColored("[", terminal);
            Tui.AppendLogColored(tag, FromSystem(BrightenColor(SaturateColor(color, 1.5f), 1.2f)));
            Tui.AppendLogColored("] ", terminal);
        }

        public void AppendTagString(bool format = true)
            => LogFormattedTag(Tag, format ? TagColor : null);

        public static void AppendErrorTag(bool format = true)
            => LogFormattedTag("ERROR", format ? ErrorTagColor : null);

        public static void AppendWarnTag(bool format = true)
            => LogFormattedTag("WARN", format ? WarnTagColor : null);

        public static void AppendDebugTag(bool format = true)
            => LogFormattedTag("DEBUG", format ? DebugTagColor : null);

        #endregion

        #region Core Logging

        // Pad console
#pragma warning disable CA1822
        public static void PadConsole(int count)
        {
            for (var i = 0; i < count; i++)
                Tui.AppendLogRaw("");
        }

        public void Newline(int count = 1) => PadConsole(count);
#pragma warning restore CA1822

        private void LogInternal(LogStyle style, bool newline, params object?[] args)
        {
            var message = string.Join(" ", args.Select(a => a?.ToString() ?? "null"));

            lock (ConsoleLock)
            {
                // var sb = new StringBuilder();
                // sb.Append(GetTagString());
                AppendTagString();

                if ((style & LogStyle.Error) != 0) 
                    AppendErrorTag(); 
                    // sb.Append(GetErrorTag());
                if ((style & LogStyle.Warn) != 0) 
                    AppendWarnTag(); 
                    // sb.Append(GetWarnTag());
                if ((style & LogStyle.Debug) != 0 && IsDebug) 
                    AppendDebugTag(); 
                    // sb.Append(GetDebugTag());

                if (newline)
                    Tui.AppendLog(message);
                else
                    Tui.AppendLogRaw(message);

                // sb.Append(message);
                // if (newline)
                //     Tui.AppendLogAnsi(sb .ToString() + "\n");
                // else
                //     Tui.AppendLogAnsi(sb.ToString()); // Terminal.Gui TextField doesn't do inline, but keeping signature
            }
        }

        // Info
        public void Info(params object?[] args) => LogInternal(LogStyle.Info, true, args);
        public void Info(bool newline, params object?[] args) => LogInternal(LogStyle.Info, newline, args);

        public void InfoSplit(string multiline)
        {
            foreach (var line in multiline.Split('\n'))
                Log(LogStyle.Info, line);
        }

        // Warn
        public void Warn(params object?[] args) => LogInternal(LogStyle.Warn, true, args);
        public void Warn(bool newline, params object?[] args) => LogInternal(LogStyle.Warn, newline, args);

        // Error
        public void Error(params object?[] args) => LogInternal(LogStyle.Error, true, args);
        public void Error(bool newline, params object?[] args) => LogInternal(LogStyle.Error, newline, args);

        // Debug
        public void Debug(params object?[] args) => LogInternal(LogStyle.Debug, true, args);
        public void Debug(bool newline, params object?[] args) => LogInternal(LogStyle.Debug, newline, args);

        // Combined styles
        public void Log(LogStyle style, params object?[] args) => LogInternal(style, true, args);
        public void Log(LogStyle style, bool newline, params object?[] args) => LogInternal(style, newline, args);

        #endregion
    }
}