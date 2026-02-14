using System.Collections;
using System.Collections.ObjectModel;
using System.IO.Ports;
using GENE.Nodes;
using GENE.Nodes.Primitive;

namespace GENE.Basic.Nodes.Custom;

public struct IrButton(byte x, byte y)
{
    public bool Usable = true;
    public byte X = x;
    public byte Y = y;
}

public class IrButtonMapping : ReadOnlyDictionary<string, IrButton>
{
    public IrButtonMapping(Dictionary<string, IrButton> content) : base(content)
    {
        var buttonArray = Values.ToArray();
        var width = buttonArray.Select(btn => btn.X).Max() + 1;
        var height = buttonArray.Select(btn => btn.Y).Max() + 1;

        Mapped = new IrButton[height][];
        for (var row = 0; row < height; row++)
        {
            Mapped[row] = new IrButton[width];
            for (var col = 0; col < width; col++)
            {
                Mapped[row][col] = buttonArray.First(b => b.X == col && b.Y == row);
            }
        }
    }


    public readonly IrButton[][] Mapped;
    public IrButton this[int x, int y] => Mapped[x][y];
}

public class IrRemote(string serialPort, IrButtonMapping mapping, string? name = null) : AsyncNode
{
    public override string Name => name ?? "IrRemote";

    public bool IsConnected => Connection is { IsOpen: true };
    public SerialPort? Connection { get; protected set; }

    protected override void Initialize()
    {   
        using var serial = new SerialPort(serialPort, 2000000, Parity.None, 8, StopBits.One);
        serial.Open();

        Connection = serial;
    }

    public event Action<IrButton>? ButtonPressed;
    protected override async Task Run()
    {
        while (true)
        {
            if(!IsConnected)
                await Task.Delay(1000);

            var pairRaw = Connection?.ReadLine();
            if(pairRaw is null) continue;

            var pair = pairRaw.Split(',');
            if (!byte.TryParse(pair[0], out var x)) continue;
            if (!byte.TryParse(pair[1], out var y)) continue;
            
            ButtonPressed?.Invoke(new IrButton(x, y));
        }
    }
}