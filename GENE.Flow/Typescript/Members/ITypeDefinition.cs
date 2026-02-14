using System.Data.SqlTypes;

namespace GENE.Flow.Typescript.Members;

public interface ITypeDefinition
{
    public bool Nullable => false;
    public string FriendlyName { get; }
    internal string NullExtension() => Nullable ? " | null" : string.Empty;
}

public interface ITypeDefinition<T> : ITypeDefinition
{
    public T Construct(string serialized);
}

public interface IMember
{
    public string Name { get; }
    public ITypeDefinition Type { get; }
    public string ToString();
}

public record Member(string Name, ITypeDefinition Type) : IMember
{
    public override string ToString() => $"{Name}: {Type.FriendlyName}{Type.NullExtension()}";
}

public record Method(string Name, ITypeDefinition Type) : Member(Name, Type)
{
    public override string ToString() => base.ToString();
}
public record Setter(string Name, ITypeDefinition Type) : Member(Name, Type)
{
    public override string ToString() => $"set_{Name}: {Type.FriendlyName}{Type.NullExtension()}";
}
public record Getter(string Name, ITypeDefinition Type) : Member(Name, Type)
{
    public override string ToString() => $"get_{Name}: {Type.FriendlyName}{Type.NullExtension()}";
}

// public record Member<T>(string Name, ITypeDefinition<T> Type) : IMember
// {
//     public override string ToString() => $"{Name}: {Type.FriendlyName}{Type.NullExtension()}";
//     public T Construct(string serialized) => Type.Construct(serialized);
// }

