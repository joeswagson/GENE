using System.Reflection;
using System.Text;
using GENE.Flow.Typescript.Members;
using GENE.Flow.Typescript.Members.Data;
using GENE.Nodes;
using JetBrains.Annotations;

namespace GENE.Flow.Typescript;

[AttributeUsage(AttributeTargets.Method)]
public class NodeMemberAttribute(string? nameOverride = null) : Attribute;

/// <summary>
/// Converts a C# Runtime <see cref="Type"/> into a TypeScript typedef for a GENE node.
/// </summary>
public static class Typist
{
    // wtf is this rider
    private static readonly Dictionary<Type, ITypeDefinition> Simples = new() {
        {
            typeof(bool), SimpleTypes.Bool
        }, {
            typeof(byte), SimpleTypes.Byte
        }, {
            typeof(sbyte), SimpleTypes.SByte
        }, {
            typeof(char), SimpleTypes.Char
        }, {
            typeof(decimal), SimpleTypes.Decimal
        }, {
            typeof(double), SimpleTypes.Double
        }, {
            typeof(float), SimpleTypes.Float
        }, {
            typeof(int), SimpleTypes.Int32
        }, {
            typeof(uint), SimpleTypes.UInt32
        }, {
            typeof(nint), SimpleTypes.IntPtr
        }, {
            typeof(nuint), SimpleTypes.UIntPtr
        }, {
            typeof(long), SimpleTypes.Int64
        }, {
            typeof(ulong), SimpleTypes.UInt64
        }, {
            typeof(short), SimpleTypes.Int16
        }, {
            typeof(ushort), SimpleTypes.UInt16
        }, {
            typeof(string), SimpleTypes.String
        }, {
            typeof(object), SimpleTypes.Object
        }
    };

    private static readonly Dictionary<Type, ITypeDefinition> NullableSimples = new() {
        {
            typeof(bool), SimpleTypes.NullableBool
        }, {
            typeof(byte), SimpleTypes.NullableByte
        }, {
            typeof(sbyte), SimpleTypes.NullableSByte
        }, {
            typeof(char), SimpleTypes.NullableChar
        }, {
            typeof(decimal), SimpleTypes.NullableDecimal
        }, {
            typeof(double), SimpleTypes.NullableDouble
        }, {
            typeof(float), SimpleTypes.NullableFloat
        }, {
            typeof(int), SimpleTypes.NullableInt32
        }, {
            typeof(uint), SimpleTypes.NullableUInt32
        }, {
            typeof(nint), SimpleTypes.NullableIntPtr
        }, {
            typeof(nuint), SimpleTypes.NullableUIntPtr
        }, {
            typeof(long), SimpleTypes.NullableInt64
        }, {
            typeof(ulong), SimpleTypes.NullableUInt64
        }, {
            typeof(short), SimpleTypes.NullableInt16
        }, {
            typeof(ushort), SimpleTypes.NullableUInt16
        }, {
            typeof(string), SimpleTypes.NullableString
        }, {
            typeof(object), SimpleTypes.NullableObject
        }
    };

    // Assuming TupleDefinition is defined in the namespace GENE.Flow.Typescript.Members.Data
    // or you can define it here if it doesn't exist yet.
    public class TupleDefinition(ITypeDefinition[] elementDefinitions, bool nullable = false) : ITypeDefinition
    {
        public bool Nullable => nullable;
        public string FriendlyName => $"[{string.Join(", ", elementDefinitions.Select(t => t.ToString()))}]";
    }

    /// <summary>
    /// Converts a type into two buckets of method definitions for typescript-compliant node interfaces. 
    /// </summary>
    public class NodeConverter : ITypeScriptDefinition
    {
        private readonly Type _type;
        public Type[]? Interfaces;
        public IMember[] Signal;
        public IMember[] Output;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"interface {_type.Name} {{");

            sb.AppendLine("\tsignals: {");
            foreach (var signal in Signal)
                sb.AppendLine($"\t\t{signal.ToString()};");
            sb.AppendLine("\t}");

            sb.AppendLine("\toutputs: {");
            foreach (var output in Output)
                sb.AppendLine($"\t\t{output.ToString()};");
            sb.AppendLine("\t}");

            sb.Append('}');
            return sb.ToString();
        }

        public NodeConverter(Type type, Type[]? interfaces = null)
        {
            _type = type;
            Interfaces = interfaces;
            
            List<IMember> signal = [];
            List<IMember> output = [];

            var methods = type.GetMethods(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.DeclaredOnly
            );

            var grouped = methods
                .GroupBy(m =>
                    m.Name.StartsWith("get_") ? m.Name[4..] :
                    m.Name.StartsWith("set_") ? m.Name[4..] :
                    m.Name
                );

            foreach (var group in grouped)
            {
                var getter = group.FirstOrDefault(m => m.Name.StartsWith("get_"));
                var setter = group.FirstOrDefault(m => m.Name.StartsWith("set_"));

                // steal nullability from setter if possible
                var sharedNullability =
                    setter != null
                        ? _nullability.Create(setter.GetParameters()[0])
                        : getter != null
                            ? _nullability.Create(getter.ReturnParameter)
                            : null;

                if (setter != null)
                {
                    var p = setter.GetParameters()[0];
                    var typeDef = FindTypeDefinition(p.ParameterType, sharedNullability);
                    signal.Add(new Setter(group.Key, typeDef));
                }

                if (getter != null)
                {
                    var typeDef = FindTypeDefinition(getter.ReturnType, sharedNullability);
                    output.Add(new Getter(group.Key, typeDef));
                }

                foreach (var m in group)
                {
                    if (m == getter || m == setter)
                        continue;

                    var member = Method(m);
                    if (member == null)
                        continue;

                    if (m.ReturnType == typeof(void) || m.GetParameters().Length > 0)
                        signal.Add(member);
                    else
                        output.Add(member);
                }
            }

            Signal = [.. signal];
            Output = [.. output];
        }


        private IMember? CreateMember(string name, Type type) => new Member(name, FindTypeDefinition(type));

        private ITypeDefinition FindTypeDefinition(Type type, NullabilityInfo? nullability = null)
        {
            var nullable =
                Nullable.GetUnderlyingType(type) != null || // value types
                nullability?.ReadState == NullabilityState.Nullable;
            var rootType = type.IsArray ? type.GetElementType() : type;
            // Console.WriteLine($"type: {type.FullName}, nullable: {nullable}");

            // basic types
            var pool = nullable ? NullableSimples : Simples;
            if (pool.TryGetValue(rootType!, out var simpleDef))
                return simpleDef;

            // tuples (will become lists)
            if (type.IsGenericType
                && (type.GetGenericTypeDefinition() == typeof(ValueTuple<>)
                    || type.GetGenericTypeDefinition() == typeof(Tuple<>)))
            {
                var genericArgs = type.GetGenericArguments();
                var definitions = genericArgs.Select(arg => FindTypeDefinition(arg)).ToArray();
                return new TupleDefinition(definitions, nullable);
            }

            if (type.IsArray)
            {
                var elemType = type.GetElementType()!;
                var elemNull = nullability?.ElementType;
                return new ArrayType(
                    FindTypeDefinition(elemType, elemNull),
                    nullable
                );
            }
            
            if(Interfaces?.Any(t=>t.Name == type.Name) ?? false)
                return new SimpleType<object>(type.Name, s=>s, nullable);

            return nullable ? AnyType.NullInstance : AnyType.Instance;
        }


        private IMember? Field(FieldInfo field) => CreateMember(field.Name, field.FieldType);
        private IMember? Property(PropertyInfo property) => CreateMember(property.Name, property.PropertyType);

        private readonly NullabilityInfoContext _nullability = new();

        private Member? Method(MethodInfo method)
        {
            var parameters = method.GetParameters();
            var paramMembers = parameters
                .Select(p =>
                    new Member(p.Name ?? "unknown", FindTypeDefinition(p.ParameterType, _nullability.Create(p))))
                .ToArray();

            ITypeDefinition? returnType = null;
            if (method.ReturnType != typeof(void))
            {
                var rn = _nullability.Create(method.ReturnParameter);
                returnType = FindTypeDefinition(method.ReturnType, rn);
            }


            var functionType = new FunctionType(paramMembers, returnType);
            return new Member(method.Name, functionType);
        }
    }

    public static TypistNode Convert(Type type, params Type[] interfaces) => new(type, interfaces);
    public class TypistNode
    {
        public readonly NodeConverter Root;
        public readonly NodeConverter? Payload;
        public readonly NodeConverter? Response;

        public TypistNode(Type type, Type[]? interfaces = null)
        {
            if (type.IsAssignableFrom(typeof(INode)))
                throw new ArgumentException($"{type.Name} does not implement {nameof(INode)}.");

            Root = new NodeConverter(type, interfaces);
            var inode = type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType &&
                                     i.GetGenericTypeDefinition() == typeof(INode<,>));
            if (inode is null) return;
            var genericTypes = inode.GetGenericArguments();
            Payload = new NodeConverter(genericTypes[0], interfaces);
            Response = new NodeConverter(genericTypes[1], interfaces);
        }
    }
}