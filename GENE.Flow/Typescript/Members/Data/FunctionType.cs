using System.Text;
using JetBrains.Annotations;

namespace GENE.Flow.Typescript.Members.Data;

public class FunctionType(Member[] @params, ITypeDefinition? @return) : ITypeDefinition<object>
{
    public string FriendlyName => field ??= ToString();
    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append('(');
        sb.AppendJoin(", ", @params.Select(param => param.ToString()));
        sb.Append(") => ");
        sb.Append(@return?.FriendlyName ?? "void");

        return sb.ToString();
    }

    public object Construct(string serialized) => null!;
}
