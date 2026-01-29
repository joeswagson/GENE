namespace GENE.Flow.Typescript.Members.Data;

public class AnyType(bool nullable = false) : ITypeDefinition
{
    public static readonly AnyType Instance = new();
    public static readonly AnyType NullInstance = new(true);
    
    public string FriendlyName => "any";
    public bool Nullable => nullable;
}