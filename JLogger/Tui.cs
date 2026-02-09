using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using Terminal.Gui;
using Terminal.Gui.App;
using Terminal.Gui.Drawing;
using Terminal.Gui.Drivers;
using Terminal.Gui.Input;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
using Attribute = Terminal.Gui.Drawing.Attribute;
using Color = Terminal.Gui.Drawing.Color;

namespace JLogger;

public static class Tui
{
    public static readonly Color Background = Color.Black;
    public static Window AppWindow = null!;
    public static TextView LogView = null!;
    public static TextField Input = null!;

    public static void Run()
    {
        if(_app is null)
            throw new InvalidOperationException("Application is null.");
        
        _app.Init();
        var top = _app.TopRunnableView;

        var win = AppWindow = new Window()
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        // SCROLLING LOG
        LogView = new TextView
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill() - 1,
            ReadOnly = true,
            WordWrap = true
        };

        // INPUT FIELD
        Input = new TextField()
        {
            X = 6,
            Y = Pos.Bottom(LogView),
            Width = Dim.Fill(),
            Height = 1
        };

        // PROMPT LABEL
        var prompt = new Label()
        {
            Text = "cmd>",
            X = 0,
            Y = Pos.Bottom(LogView)
        };

        Input.KeyDown += OnKeyPress;

        win.Add(LogView, prompt, Input);
        top?.Add(win);

        // win.SetScheme(new Scheme(win.GetScheme()) {
        //     Normal = new Attribute(
        //         Color.White, 
        //         Color.Black
        //     )
        // });
        
        _app?.Invoke(() =>
        {
            Input.SetFocus();
        });

        _app?.Run(win);
    }

    private static void OnKeyPress(object? sender, Key? e)
    {
        if (e == Key.Enter)
        {
            var text = Input.Text;
            Input.Text = "";
            Input.Move(0,0);

            CommandDispatcher.Enqueue(text!);
            e.Handled = true;
        }
    }
    public static void AppendLogRaw(string text)
    {
        _app?.Invoke(() =>
        {
            LogView.Text += text;
            LogView.MoveEnd();
        });
    }
    
    
    private static readonly Dictionary<(Color fg, Color bg), Attribute> AttrCache = new();

    private static Attribute GetAttr(Color fg, Color bg)
    {
        var key = (fg, bg);
        if (!AttrCache.TryGetValue(key, out var attr))
        {
            attr = new Attribute(fg, bg);
            AttrCache[key] = attr;
        }
        return attr;
    }

    public static void AppendLogAnsi(string text)
    {
        _app?.Invoke(() =>
        {
            Debug.WriteLine(text);
            // LogView.SetScheme(new Scheme(){ Normal = GetAttr(fg, bg ?? Color.Gray)});
            // LogView.Text += text;
            _app.Driver?.Move(LogView.CurrentColumn, LogView.CurrentRow);
            // _app.Driver.CurrentAttribute(text);
            LogView.DrawText();
            LogView.MoveEnd();
        });
    }
    
    public static void AppendLogColored(string text, Color fg, Color? bg = null)
    {
        _app?.Invoke(() =>
        {
            Debug.WriteLine("{0}, {1}", text, fg.ToString());
            // LogView.SetScheme(new Scheme(){ Normal = GetAttr(fg, bg ?? Color.Gray)});
            _app.Driver?.CurrentAttribute = GetAttr(fg, bg ?? Color.Gray);
            LogView.Text += text;
            LogView.DrawText();
            LogView.MoveEnd();
        });
    }

    public static void AppendLog(string text)
    {
        _app?.Invoke(() =>
        {
            LogView.Text += text + "\n";
            LogView.MoveEnd();
            Input.SetFocus(); // keep focus in input
        });
    }

    private static IApplication? _app;
    public static void Create()
    {
        _app = Application.Create();
    }
}

public static class CommandDispatcher
{
    private static readonly BlockingCollection<string> Queue = new();

    public static void Enqueue(string cmd)
        => Queue.Add(cmd);

    public static string Take(CancellationToken token)
        => Queue.Take(token);
}
