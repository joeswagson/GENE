namespace GENE.Flow.Typescript.Members.Data;

public class ArrayType(ITypeDefinition source, bool nullable) : ITypeDefinition
{
    public bool Nullable => nullable;
    public string FriendlyName => $"{source.FriendlyName}[]";
}