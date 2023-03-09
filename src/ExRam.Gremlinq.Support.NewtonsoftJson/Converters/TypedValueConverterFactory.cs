using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Process.Traversal;
using System.Numerics;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class TypedValueConverterFactory : IConverterFactory
    {
        private static readonly Dictionary<string, Type> GraphSONTypes = new()
        {
            { "g:Int32", typeof(int) },
            { "g:Int64", typeof(long) },
            { "g:Float", typeof(float) },
            { "g:Double", typeof(double) },
            { "g:Direction", typeof(Direction) },
            { "g:Merge", typeof(Merge) },
            { "g:UUID", typeof(Guid) },
            { "g:Date", typeof(DateTimeOffset) },
            { "g:Timestamp", typeof(DateTimeOffset) },
            { "g:T", typeof(T) },

            //Extended
            { "gx:BigDecimal", typeof(decimal) },
            { "gx:Duration", typeof(TimeSpan) },
            { "gx:BigInteger", typeof(BigInteger) },
            { "gx:Byte",typeof(byte) },
            { "gx:ByteBuffer", typeof(byte[]) },
            { "gx:Char", typeof(char) },
            { "gx:Int16", typeof(short) }
        };

        public sealed class TypedValueConverter<TTarget> : IConverter<JObject, TTarget>
        {
            public bool TryConvert(JObject serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                if (serialized.TryGetValue("@type", out var typeName) && serialized.TryGetValue("@value", out var valueToken))
                {
                    if (typeName.Type == JTokenType.String && typeName.Value<string>() is { } typeNameString && GraphSONTypes.TryGetValue(typeNameString, out var moreSpecificType))
                    {
                        if (typeof(TTarget) != moreSpecificType && typeof(TTarget).IsAssignableFrom(moreSpecificType))
                        {
                            if (recurse.TryDeserialize(moreSpecificType).From(valueToken, environment) is TTarget target)
                            {
                                value = target;
                                return true;
                            }
                        }
                    }

                    return recurse.TryTransform(valueToken, environment, out value);
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
        {
            return typeof(TSource) == typeof(JObject)
                ? (IConverter<TSource, TTarget>)(object)new TypedValueConverter<TTarget>()
                : default;
        }
    }
}
