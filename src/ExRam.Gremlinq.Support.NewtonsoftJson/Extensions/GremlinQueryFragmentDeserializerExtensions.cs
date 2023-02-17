using Gremlin.Net.Process.Traversal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Xml;
using System.Numerics;
using ExRam.Gremlinq.Core.GraphElements;
using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public static class GremlinQueryFragmentDeserializerExtensions
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

        private sealed class NewtonsoftJsonSerializerDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class NewtonsoftJsonSerializerDeserializationTransformation<TSerialized, TRequested> : IDeserializationTransformation<TSerialized, TRequested>
                where TSerialized : JToken
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (serialized is TRequested alreadyRequestedValue)
                    {
                        value = alreadyRequestedValue;
                        return true;
                    }

                    if (serialized.ToObject<TRequested>(environment.GetJsonSerializer(recurse)) is { } requestedValue)
                    {
                        value = requestedValue;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(JToken).IsAssignableFrom(typeof(TSerialized))
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(NewtonsoftJsonSerializerDeserializationTransformation<,>).MakeGenericType(typeof(TSerialized), typeof(TRequested)))
                    : null;
            }
        }

        private sealed class VertexOrEdgeDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class VertexOrEdgeDeserializationTransformation<TSerialized, TRequested> : IDeserializationTransformation<TSerialized, TRequested>
                where TSerialized : JObject
            {
                public bool Transform(TSerialized jObject, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (jObject.TryGetValue("id", StringComparison.OrdinalIgnoreCase, out var idToken) && jObject.TryGetValue("label", StringComparison.OrdinalIgnoreCase, out var labelToken) && labelToken.Type == JTokenType.String && jObject.TryGetValue("properties", out var propertiesToken) && propertiesToken is JObject propertiesObject)
                    {
                        if (recurse.TryDeserialize(propertiesObject, environment, out value))
                        {
                            value.SetIdAndLabel(idToken, labelToken, environment, recurse);
                            return true;
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return (typeof(JObject).IsAssignableFrom(typeof(TSerialized)) && !typeof(TRequested).IsAssignableFrom(typeof(TSerialized)) && !typeof(Property).IsAssignableFrom(typeof(TRequested)))
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(VertexOrEdgeDeserializationTransformation<,>).MakeGenericType(typeof(TSerialized), typeof(TRequested)))
                    : default;
            }
        }

        private sealed class SingleItemArrayFallbackDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class SingleItemArrayFallbackDeserializationTransformation<TSerialized, TRequestedArray, TRequestedArrayItem> : IDeserializationTransformation<TSerialized, TRequestedArray>
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequestedArray? value)
                {
                    if (!environment.GetCache().FastNativeTypes.ContainsKey(typeof(TRequestedArray)) && recurse.TryDeserialize<TSerialized, TRequestedArrayItem>(serialized, environment, out var typedValue))
                    {
                        value = (TRequestedArray)(object)new[] { typedValue };
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TRequested).IsArray
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(SingleItemArrayFallbackDeserializationTransformation<,,>).MakeGenericType(typeof(TSerialized), typeof(TRequested), typeof(TRequested).GetElementType()!))
                    : default;
            }
        }

        private sealed class NullableDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class NullableDeserializationTransformation<TSerialized, TRequested> : IDeserializationTransformation<TSerialized, TRequested?>
                where TSerialized : JToken
                where TRequested : struct
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (serialized.Type == JTokenType.Null)
                    {
                        value = default(TRequested);
                        return true;
                    }

                    if (recurse.TryDeserialize<TSerialized, TRequested>(serialized, environment, out var requestedValue))
                    {
                        value = requestedValue;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(JToken).IsAssignableFrom(typeof(TSerialized)) && typeof(TRequested).IsGenericType && typeof(TRequested).GetGenericTypeDefinition() == typeof(Nullable<>)
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(NullableDeserializationTransformation<,>).MakeGenericType(typeof(TSerialized), typeof(TRequested).GetGenericArguments()[0]))
                    : default;
            }
        }

        private sealed class PropertyDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class PropertyDeserializationTransformation<TSerialized, TRequestedProperty, TRequestedPropertyValue> : IDeserializationTransformation<TSerialized, TRequestedProperty>
                where TSerialized : JValue
                where TRequestedProperty : Property
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequestedProperty? value)
                {
                    if (recurse.TryDeserialize<TSerialized, TRequestedPropertyValue>(serialized, environment, out var propertyValue))
                    {
                        //TODO: Improvement opportunity.

                        if (Activator.CreateInstance(typeof(TRequestedProperty), propertyValue) is TRequestedProperty requestedProperty)
                        {
                            value = requestedProperty;
                            return true;
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(JValue).IsAssignableFrom(typeof(TSerialized)) && typeof(Property).IsAssignableFrom(typeof(TRequested)) && typeof(TRequested).IsGenericType
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(PropertyDeserializationTransformation<,,>).MakeGenericType(typeof(TSerialized), typeof(TRequested), typeof(TRequested).GetGenericArguments()[0]))
                    : default;
            }
        }

        private sealed class NativeTypeDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            public sealed class NativeTypeDeserializationTransformation<TSerialized, TRequested> : IDeserializationTransformation<TSerialized, TRequested>
                where TSerialized : JValue
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (serialized.Value is TRequested serializedValue)
                    {
                        value = serializedValue;
                        return true;
                    }

                    if (serialized.Value is { } otherSerializedValue)
                    {
                        var type = typeof(TRequested);

                        if (type == typeof(int) || type == typeof(byte) || type == typeof(sbyte) || type == typeof(ushort) || type == typeof(short) || type == typeof(uint) || type == typeof(ulong) || type == typeof(long) || type == typeof(float) || type == typeof(double))
                        {
                            value = (TRequested)Convert.ChangeType(otherSerializedValue, type);
                            return true;
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(JValue).IsAssignableFrom(typeof(TSerialized))
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(NativeTypeDeserializationTransformation<,>).MakeGenericType(typeof(TSerialized), typeof(TRequested)))
                    : default;
            }
        }

        private sealed class ExpandoObjectDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class ExpandoObjectDeserializationTransformation<TSerialized, TRequested> : IDeserializationTransformation<TSerialized, TRequested>
                where TSerialized : JObject
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (recurse.TryDeserialize<TSerialized, JObject>(serialized, environment, out var processedFragment))
                    {
                        var expando = new ExpandoObject();

                        foreach (var property in processedFragment)
                        {
                            if (property.Value is { } propertyValue && recurse.TryDeserialize<JToken, object>(propertyValue, environment, out var item))
                                expando.TryAdd(property.Key, item);
                        }

                        value = (TRequested)(object)expando;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(JObject).IsAssignableFrom(typeof(TSerialized)) && typeof(TRequested).IsAssignableFrom(typeof(ExpandoObject))
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(ExpandoObjectDeserializationTransformation<,>).MakeGenericType(typeof(TSerialized), typeof(TRequested)))
                    : default;
            }
        }

        private sealed class LabelLookupDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class LabelLookupDeserializationTransformation<TSerialized, TRequested> : IDeserializationTransformation<TSerialized, TRequested>
                where TSerialized : JObject
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    // Elements
                    var modelTypes = environment.GetCache().ModelTypesForLabels;
                    var label = serialized["label"]?.ToString();

                    var modelType = label != null && modelTypes.TryGetValue(label, out var types)
                        ? types.FirstOrDefault(typeof(TRequested).IsAssignableFrom)
                        : default;

                    if (modelType != null && modelType != typeof(TRequested))
                    {
                        if (recurse.TryDeserialize(modelType).From(serialized, environment) is TRequested requested)
                        {
                            value = requested;
                            return true;
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(JObject).IsAssignableFrom(typeof(TSerialized)) && !typeof(TSerialized).IsSealed
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(LabelLookupDeserializationTransformation<,>).MakeGenericType(typeof(TSerialized), typeof(TRequested)))
                    : default;
            }
        }

        private sealed class VertexPropertyExtractDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class VertexPropertyExtractDeserializationTransformation<TSerialized, TRequested> : IDeserializationTransformation<TSerialized, TRequested>
                where TSerialized : JObject
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    var nativeTypes = environment.GetCache().FastNativeTypes;

                    if (nativeTypes.ContainsKey(typeof(TRequested)) || typeof(TRequested).IsEnum && nativeTypes.ContainsKey(typeof(TRequested).GetEnumUnderlyingType()))
                    {
                        if (serialized.TryGetValue("value", out var valueToken))
                        {
                            if (recurse.TryDeserialize(valueToken, environment, out value))
                                return true;
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(JObject).IsAssignableFrom(typeof(TSerialized))
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(VertexPropertyExtractDeserializationTransformation<,>).MakeGenericType(typeof(TSerialized), typeof(TRequested)))
                    : default;
            }
        }

        private sealed class TypedValueDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            public sealed class TypedValueDeserializationTransformation<TSerialized, TRequested> : IDeserializationTransformation<TSerialized, TRequested>
                where TSerialized : JObject
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (serialized.TryGetValue("@type", out var typeName) && serialized.TryGetValue("@value", out var valueToken))
                    {
                        if (typeName.Type == JTokenType.String && typeName.Value<string>() is { } typeNameString && GraphSONTypes.TryGetValue(typeNameString, out var moreSpecificType))
                        {
                            if (typeof(TRequested) != moreSpecificType && typeof(TRequested).IsAssignableFrom(moreSpecificType))
                            {
                                if (recurse.TryDeserialize(moreSpecificType).From(valueToken, environment) is TRequested requested)
                                {
                                    value = requested;
                                    return true;
                                }
                            }
                        }

                        return recurse.TryDeserialize(valueToken, environment, out value);
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(JObject).IsAssignableFrom(typeof(TSerialized))
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(TypedValueDeserializationTransformation<,>).MakeGenericType(typeof(TSerialized), typeof(TRequested)))
                    : default;
            }
        }

        private sealed class ConvertMapsDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class ConvertMapsDeserializationTransformation<TSerialized, TRequested> : IDeserializationTransformation<TSerialized, TRequested>
                where TSerialized : JObject
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (serialized.TryGetValue("@type", out var nestedType) && "g:Map".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase))
                    {
                        if (serialized.TryGetValue("@value", out var valueToken) && valueToken is JArray mapArray)
                        {
                            var retObject = new JObject();

                            for (var i = 0; i < mapArray.Count / 2; i++)
                            {
                                if (mapArray[i * 2] is JValue { Type: JTokenType.String } key)
                                    retObject.Add(key.Value<string>()!, mapArray[i * 2 + 1]);
                            }

                            return recurse.TryDeserialize(retObject, environment, out value);
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(JObject).IsAssignableFrom(typeof(TSerialized))
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(ConvertMapsDeserializationTransformation<,>).MakeGenericType(typeof(TSerialized), typeof(TRequested)))
                    : default;
            }
        }

        private sealed class BulkSetDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class BulkSetDeserializationTransformation<TSerialized, TRequestedArray, TRequestedItem> : IDeserializationTransformation<TSerialized, TRequestedArray>
                where TSerialized : JObject
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequestedArray? value)
                {
                    if (!environment.GetCache().FastNativeTypes.ContainsKey(typeof(TRequestedArray)))
                    {
                        if (serialized.TryGetValue("@type", out var typeToken) && "g:BulkSet".Equals(typeToken.Value<string>(), StringComparison.OrdinalIgnoreCase))
                        {
                            if (serialized.TryGetValue("@value", out var valueToken) && valueToken is JArray setArray)
                            {
                                var array = new List<TRequestedItem>();

                                for (var i = 0; i < setArray.Count; i += 2)
                                {
                                    if (recurse.TryDeserialize<JToken, TRequestedItem>(setArray[i], environment, out var element))
                                    {
                                        if (recurse.TryDeserialize<JToken, int>(setArray[i + 1], environment, out var bulk) && bulk != 1)
                                        {
                                            for (var j = 0; j < bulk; j++)
                                            {
                                                array.Add(element);
                                            }
                                        }
                                        else
                                            array.Add(element);
                                    }
                                }

                                value = (TRequestedArray)(object)array.ToArray();
                                return true;
                            }
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TRequested).IsArray && typeof(JObject).IsAssignableFrom(typeof(TSerialized))
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(BulkSetDeserializationTransformation<,,>).MakeGenericType(typeof(TSerialized), typeof(TRequested), typeof(TRequested).GetElementType()!))
                    : default;
            }
        }

        private sealed class ArrayExtractDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class ArrayExtractDeserializationTransformation<TSerialized, TRequested> : IDeserializationTransformation<TSerialized, TRequested>
                where TSerialized : JArray
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if ((!typeof(TRequested).IsArray || environment.GetCache().FastNativeTypes.ContainsKey(typeof(TRequested))) && !typeof(TRequested).IsInstanceOfType(serialized))
                    {
                        if (serialized.Count != 1)
                        {
                            value = serialized.Count == 0 && typeof(TRequested).IsClass
                                ? default!  //TODO: Drop NotNullWhen(true) ?
                                : throw new JsonReaderException($"Cannot convert array\r\n\r\n{serialized}\r\n\r\nto scalar value of type {typeof(TRequested)}.");

                            return true;
                        }

                        return recurse.TryDeserialize(serialized[0], environment, out value);
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(JArray).IsAssignableFrom(typeof(TSerialized))
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(ArrayExtractDeserializationTransformation<,>).MakeGenericType(typeof(TSerialized), typeof(TRequested)))
                    : default;
            }
        }

        private sealed class ArrayLiftingDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class ArrayLiftingDeserializationTransformation<TSerialized, TRequested> : IDeserializationTransformation<TSerialized, TRequested>
                where TSerialized : JArray
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (recurse.TryDeserialize<TSerialized, object[]>(serialized, environment, out var requested))
                    {
                        value = (TRequested)(object)requested;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(JArray).IsAssignableFrom(typeof(TSerialized)) && typeof(TRequested).IsAssignableFrom(typeof(object[]))
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(ArrayLiftingDeserializationTransformation<,>).MakeGenericType(typeof(TSerialized), typeof(TRequested)))
                    : default;
            }
        }

        private sealed class TraverserDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class TraverserDeserializationTransformation<TSerialized, TRequestedArray, TRequestedItem> : IDeserializationTransformation<TSerialized, TRequestedArray>
                where TSerialized : JArray
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequestedArray? value)
                {
                    if (!environment.GetCache().FastNativeTypes.ContainsKey(typeof(TRequestedArray)))
                    {
                        var array = default(List<TRequestedItem>);

                        for (var i = 0; i < serialized.Count; i++)
                        {
                            if (serialized[i] is JObject traverserObject && traverserObject.TryExpandTraverser<TRequestedItem>(environment, recurse) is { } enumerable)
                            {
                                array ??= new List<TRequestedItem>(serialized.Count);

                                foreach (var item1 in enumerable)
                                {
                                    array.Add(item1);
                                }
                            }
                            else if (recurse.TryDeserialize<JToken, TRequestedItem>(serialized[i], environment, out var item2))
                            {
                                array ??= new List<TRequestedItem>(serialized.Count);

                                array.Add(item2);
                            }
                        }

                        value = (TRequestedArray)(object)(array?.ToArray() ?? Array.Empty<TRequestedItem>());
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(JArray).IsAssignableFrom(typeof(TSerialized)) && typeof(TRequested).IsArray
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(TraverserDeserializationTransformation<,,>).MakeGenericType(typeof(TSerialized), typeof(TRequested), typeof(TRequested).GetElementType()!))
                    : default;
            }
        }

        // ReSharper disable ConvertToLambdaExpression
        public static IGremlinQueryFragmentDeserializer AddNewtonsoftJson(this IGremlinQueryFragmentDeserializer deserializer) => deserializer
            .Override(new NewtonsoftJsonSerializerDeserializationTransformationFactory())
            .Override(new VertexOrEdgeDeserializationTransformationFactory())
            .Override(new SingleItemArrayFallbackDeserializationTransformationFactory())
            .Override(new NullableDeserializationTransformationFactory())
            .Override(new PropertyDeserializationTransformationFactory())
            .Override(new ExpandoObjectDeserializationTransformationFactory())  //TODO: Move
            .Override(new LabelLookupDeserializationTransformationFactory())
            .Override(new VertexPropertyExtractDeserializationTransformationFactory())
            .Override(new ArrayExtractDeserializationTransformationFactory())
            .Override(new ArrayLiftingDeserializationTransformationFactory())
            .Override(new TypedValueDeserializationTransformationFactory())
            .Override(new ConvertMapsDeserializationTransformationFactory())
            .Override(new BulkSetDeserializationTransformationFactory())
            .Override(new TraverserDeserializationTransformationFactory())
            .Override(new NativeTypeDeserializationTransformationFactory())
            .Override<JValue, TimeSpan>(static (jValue, env, recurse) => jValue.Type == JTokenType.String
                ? XmlConvert.ToTimeSpan(jValue.Value<string>()!)
                : default(TimeSpan?))
            .Override<JValue, DateTimeOffset>(static (jValue, env, recurse) =>
            {
                switch (jValue.Value)
                {
                    case DateTime dateTime:
                        return new DateTimeOffset(dateTime);
                    case DateTimeOffset dateTimeOffset:
                        return dateTimeOffset;
                    default:
                    {
                        if (jValue.Type == JTokenType.Integer)
                            return DateTimeOffset.FromUnixTimeMilliseconds(jValue.Value<long>());

                        break;
                    }
                }

                return default;
            })
            .Override<JValue, DateTime>(static (jValue, env, recurse) =>
            {
                switch (jValue.Value)
                {
                    case DateTime dateTime:
                        return dateTime;
                    case DateTimeOffset dateTimeOffset:
                        return dateTimeOffset.UtcDateTime;
                }

                if (jValue.Type == JTokenType.Integer)
                    return new DateTime(DateTimeOffset.FromUnixTimeMilliseconds(jValue.Value<long>()).Ticks, DateTimeKind.Utc);

                return default;
            })
            .Override<JValue, byte[]>(static (jValue, env, recurse) =>
            {
                return jValue.Type == JTokenType.String
                    ? Convert.FromBase64String(jValue.Value<string>()!)
                    : default;
            });
        // ReSharper restore ConvertToLambdaExpression
    }
}
