using Gremlin.Net.Process.Traversal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
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
                            expando.TryAdd(property.Key, recurse.TryDeserialize<object>().From(property.Value, environment));

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
                        var type = typeof(TRequested);

                        if (typeName.Type == JTokenType.String && typeName.Value<string>() is { } typeNameString && GraphSONTypes.TryGetValue(typeNameString, out var moreSpecificType))
                        {
                            if (type != moreSpecificType && type.IsAssignableFrom(moreSpecificType))
                                type = moreSpecificType;
                        }

                        if (recurse.TryDeserialize(type).From(valueToken, environment) is TRequested requested)
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
            //TODO: Item Parameter
            private sealed class BulkSetDeserializationTransformation<TSerialized, TRequested> : IDeserializationTransformation<TSerialized, TRequested>
                where TSerialized : JObject
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (!environment.GetCache().FastNativeTypes.ContainsKey(typeof(TRequested)))
                    {
                        var elementType = typeof(TRequested).GetElementType()!;

                        if (serialized.TryGetValue("@type", out var typeToken) && "g:BulkSet".Equals(typeToken.Value<string>(), StringComparison.OrdinalIgnoreCase))
                        {
                            if (serialized.TryGetValue("@value", out var valueToken) && valueToken is JArray setArray)
                            {
                                var array = new ArrayList();

                                for (var i = 0; i < setArray.Count; i += 2)
                                {
                                    var element = recurse.TryDeserialize(elementType).From(setArray[i], environment);
                                    var bulk = (int)recurse.TryDeserialize<int>().From(setArray[i + 1], environment)!;

                                    for (var j = 0; j < bulk; j++)
                                        array.Add(element);
                                }

                                value = (TRequested)(object)array.ToArray(elementType);
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
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(BulkSetDeserializationTransformation<,>).MakeGenericType(typeof(TSerialized), typeof(TRequested)))
                    : default;
            }
        }

        private sealed class TravsererDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            //TODO: Item Parameter
            private sealed class TraverserDeserializationTransformation<TSerialized, TRequested> : IDeserializationTransformation<TSerialized, TRequested>
                where TSerialized : JObject
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    //Traversers
                    if (!environment.GetCache().FastNativeTypes.ContainsKey(typeof(TRequested)))
                    {
                        var elementType = typeof(TRequested).GetElementType()!;

                        if (serialized.TryExpandTraverser(elementType, environment, recurse) is { } enumerable)
                        {
                            var array = new ArrayList();

                            foreach (var item in enumerable)
                            {
                                array.Add(item);
                            }

                            value = (TRequested)(object)array.ToArray(elementType);
                            return true;
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TRequested).IsArray && typeof(JObject).IsAssignableFrom(typeof(TSerialized))
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(TraverserDeserializationTransformation<,>).MakeGenericType(typeof(TSerialized), typeof(TRequested)))
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
            })
            .Override(new ExpandoObjectDeserializationTransformationFactory())  //TODO: Move
            .Override(new LabelLookupDeserializationTransformationFactory())
            .Override(new VertexPropertyExtractDeserializationTransformationFactory())
            .Override(new TypedValueDeserializationTransformationFactory())
            .Override(new ConvertMapsDeserializationTransformationFactory())
            .Override(new BulkSetDeserializationTransformationFactory())
            .Override(new TravsererDeserializationTransformationFactory())
            .Override<JArray>(static (jArray, type, env, recurse) =>
            {
                if ((!type.IsArray || env.GetCache().FastNativeTypes.ContainsKey(type)) && !type.IsInstanceOfType(jArray))
                    return jArray.Count != 1
                        ? jArray.Count == 0 && type.IsClass
                            ? default
                            : throw new JsonReaderException($"Cannot convert array\r\n\r\n{jArray}\r\n\r\nto scalar value of type {type}.")
                        : recurse.TryDeserialize(type).From(jArray[0], env);

                return default;
            })
            .Override<JArray>(static (jArray, type, env, recurse) =>
            {
                return type.IsAssignableFrom(typeof(object[])) && recurse.TryDeserialize<object[]>().From(jArray, env) is { } tokens
                    ? tokens
                    : default(object?);
            })
            .Override<JArray>(static (jArray, type, env, recurse) =>
            {
                //Traversers
                if (type.IsArray && !env.GetCache().FastNativeTypes.ContainsKey(type))
                {
                    var array = default(ArrayList);
                    var elementType = type.GetElementType()!;

                    for (var i = 0; i < jArray.Count; i++)
                    {
                        if (jArray[i] is JObject traverserObject && traverserObject.TryExpandTraverser(elementType, env, recurse) is { } enumerable)
                        {
                            array ??= new ArrayList(jArray.Count);

                            foreach (var item1 in enumerable)
                            {
                                array.Add(item1);
                            }
                        }
                        else if (recurse.TryDeserialize(elementType).From(jArray[i], env) is { } item2)
                        {
                            array ??= new ArrayList(jArray.Count);

                            array.Add(item2);
                        }
                    }

                    return array?.ToArray(elementType) ?? Array.CreateInstance(elementType, 0);
                }

                return default;
            });
        // ReSharper restore ConvertToLambdaExpression
    }
}
