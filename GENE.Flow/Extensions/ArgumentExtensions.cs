using System.Text.Json.Nodes;

namespace GENE.Flow.Extensions;

public static class ArgumentExtensions
{
    public static T Required<T>(this JsonObject src, string key)
    {
        if (src is null)
            throw new InvalidOperationException("Attempted to call read methods without a unpackage context");

        var value = src[key];
        if(value is null)
            throw new ArgumentNullException(nameof(key), "Key was not found.");
        
        return value.GetValue<T>() ?? throw new ArgumentNullException(nameof(key), "Key was of an invalid type");
    }
}