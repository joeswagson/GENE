using JetBrains.Annotations;

namespace GENE.Flow.Typescript.Members.Data;

public sealed class SimpleType<T>(string friendlyName, Func<string, T> construct, bool nullable = false) : ITypeDefinition<T>
{
    public bool Nullable => nullable || System.Nullable.GetUnderlyingType(typeof(T)) != null;
    public string FriendlyName { get; } = friendlyName;
    public T Construct(string serialized) => construct(serialized) ?? throw new NullReferenceException($"Failed to construct type {FriendlyName} because the handler returned null.");
}

public static class SimpleTypes
{
    #region Regular

    public static readonly SimpleType<bool> Bool = new("bool", bool.Parse);
    public static readonly SimpleType<byte> Byte = new("byte", byte.Parse);
    public static readonly SimpleType<sbyte> SByte = new("sbyte", sbyte.Parse);
    public static readonly SimpleType<char> Char = new("char", char.Parse);
    public static readonly SimpleType<decimal> Decimal = new("decimal", decimal.Parse);
    public static readonly SimpleType<double> Double = new("double", double.Parse);
    public static readonly SimpleType<float> Single = new("float", float.Parse);
    public static readonly SimpleType<int> Int32 = new("int", int.Parse);
    public static readonly SimpleType<uint> UInt32 = new("uint", uint.Parse);
    public static readonly SimpleType<nint> IntPtr = new("nint", nint.Parse);
    public static readonly SimpleType<nuint> UIntPtr = new("nuint", nuint.Parse);
    public static readonly SimpleType<long> Int64 = new("long", long.Parse);
    public static readonly SimpleType<ulong> UInt64 = new("ulong", ulong.Parse);
    public static readonly SimpleType<short> Int16 = new("short", short.Parse);
    public static readonly SimpleType<ushort> UInt16 = new("ushort", ushort.Parse);
    public static readonly SimpleType<string> String = new("string", s => s);
    public static readonly SimpleType<object> Object = new("object", s => s);
    
    // I don't like the float .NET type being called a Single; pick a lane.
    public static readonly SimpleType<float> Float = Single;
    
    #endregion
    #region Nullable

    public static readonly SimpleType<bool?> NullableBool = new("bool", s => bool.Parse(s));
    public static readonly SimpleType<byte?> NullableByte = new("byte", s => byte.Parse(s));
    public static readonly SimpleType<sbyte?> NullableSByte = new("sbyte", s => sbyte.Parse(s));
    public static readonly SimpleType<char?> NullableChar = new("char", s => char.Parse(s));
    public static readonly SimpleType<decimal?> NullableDecimal = new("decimal", s => decimal.Parse(s));
    public static readonly SimpleType<double?> NullableDouble = new("double", s => double.Parse(s));
    public static readonly SimpleType<float?> NullableSingle = new("float", s => float.Parse(s));
    public static readonly SimpleType<int?> NullableInt32 = new("int", s => int.Parse(s));
    public static readonly SimpleType<uint?> NullableUInt32 = new("uint", s => uint.Parse(s));
    public static readonly SimpleType<nint?> NullableIntPtr = new("nint", s => nint.Parse(s));
    public static readonly SimpleType<nuint?> NullableUIntPtr = new("nuint", s => nuint.Parse(s));
    public static readonly SimpleType<long?> NullableInt64 = new("long", s => long.Parse(s));
    public static readonly SimpleType<ulong?> NullableUInt64 = new("ulong", s => ulong.Parse(s));
    public static readonly SimpleType<short?> NullableInt16 = new("short", s => short.Parse(s));
    public static readonly SimpleType<ushort?> NullableUInt16 = new("ushort", s => ushort.Parse(s));
    public static readonly SimpleType<string?> NullableString = new("string", s => s);
    public static readonly SimpleType<object?> NullableObject = new("object", s => s);
    
    public static readonly SimpleType<float?> NullableFloat = NullableSingle;
    
    #endregion
}
