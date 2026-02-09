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
        var width = buttonArray.Select(btn => btn.X).Max();
        var height = buttonArray.Select(btn => btn.Y).Max();

        Mapped = new IrButton[][height];
        for (var row = 0; row <= height; row++)
        {
            Mapped[row] = new IrButton[width];
            for (var col = 0; col <= width; col++)
                Mapped[row][col] = buttonArray[width * row + col];
        }
    }


    public readonly IrButton[][] Mapped;
    public IrButton this[int x, int y] => Mapped[x][y];
}

public class IrRemote(string serialPort, IrButtonMapping mapping, string? name = null) : INode
{
    public string Name => name ?? "IrRemote";

    public bool IsConnected => Connection is { IsOpen: true };
    public SerialPort? Connection { get; protected set; }

    public virtual void Initialize()
    {
        
        
        using var serial = new SerialPort(serialPort, 2000000, Parity.None, 8, StopBits.One);
        serial.Open();

        Connection = serial;
    }
}