using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Process.Traversal;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

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
            { "g:Tree", typeof(Tree<object>) },

            //Extended
            { "gx:BigDecimal", typeof(decimal) },
            { "gx:Duration", typeof(TimeSpan) },
            { "gx:BigInteger", typeof(BigInteger) },
            { "gx:Byte",typeof(byte) },
            { "gx:ByteBuffer", typeof(byte[]) },
            { "gx:Char", typeof(char) },
            { "gx:Int16", typeof(short) }
        };

        // TinkerPop's Byte is signed (TINKERPOP-3161). A server from 3.8 on is read as Gremlin.Net 3.8
        // reads it, a gx:Byte as an sbyte. Below 3.8 it stays a byte.
        private static readonly Dictionary<string, Type> TinkerPop38GraphSONTypes = new(GraphSONTypes)
        {
            ["gx:Byte"] = typeof(sbyte)
        };

        public sealed class TypedValueConverter<TTarget> : IConverter<JObject, TTarget>
        {
            private readonly IGremlinQueryEnvironment _environment;
            private readonly Dictionary<string, Type> _graphSONTypes;

            public TypedValueConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
                _graphSONTypes = environment.ServerCompatibilityRange.IsAtLeast(TinkerPopVersions.V3_8_0)
                    ? TinkerPop38GraphSONTypes
                    : GraphSONTypes;
            }

            bool IConverter<JObject, TTarget>.TryConvert(JObject serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(serialized);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                if (serialized.TryGetValue("@type", out var typeName) && typeName.Type == JTokenType.String && typeName.Value<string>() is { } typeNameString && serialized.TryGetValue("@value", out var valueToken))
                {
                    // g:BulkSet joins g:Map in being left alone: unwrapping it would hand the
                    // flat value/bulk array to the collection converters, which would read the
                    // bulks as items. EnumerableConverterFactory reads it in pairs instead.
                    if (!"g:Map".Equals(typeNameString, StringComparison.OrdinalIgnoreCase) && !"g:BulkSet".Equals(typeNameString, StringComparison.OrdinalIgnoreCase))
                    {
                        // A gx:OffsetDateTime keeps the offset it was written in. Read through the
                        // type table, its bare @value would be moved to UTC like any other string.
                        if ("gx:OffsetDateTime".Equals(typeNameString, StringComparison.OrdinalIgnoreCase) && valueToken is JValue { Value: string offsetDateTimeString } && DateTimeOffset.TryParse(offsetDateTimeString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var offsetDateTime) && offsetDateTime is TTarget offsetDateTimeTarget)
                        {
                            value = offsetDateTimeTarget;
                            return true;
                        }

                        if (_graphSONTypes.TryGetValue(typeNameString, out var moreSpecificType))
                        {
                            if (typeof(TTarget) != moreSpecificType && typeof(TTarget).IsAssignableFrom(moreSpecificType))
                            {
                                if (recurse.TryTransformTo(moreSpecificType).From(valueToken, _environment) is TTarget target)
                                {
                                    value = target;
                                    return true;
                                }
                            }
                        }

                        return recurse.TryTransform(valueToken, _environment, out value);
                    }
                }

                value = default;
                return false;
            }
        }

        IConverter<TSource, TTarget>? IConverterFactory.TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            return typeof(TSource) == typeof(JObject)
                ? Unsafe.As<IConverter<TSource, TTarget>>(new TypedValueConverter<TTarget>(environment))
                : null;
        }
    }
}
