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

        // ReSharper disable ConvertToLambdaExpression
        public static IGremlinQueryFragmentDeserializer AddNewtonsoftJson(this IGremlinQueryFragmentDeserializer deserializer) => deserializer
            .Override(new NewtonsoftJsonSerializerDeserializationTransformationFactory())
            .Override(new VertexOrEdgeDeserializationTransformationFactory())
            .Override(new SingleItemArrayFallbackDeserializationTransformationFactory())
            .Override<JToken>(static (jToken, type, env, recurse) =>
            {
                return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)
                    ? jToken.Type == JTokenType.Null
                        ? null
                        : recurse.TryDeserialize(type.GetGenericArguments()[0]).From(jToken, env)
                    : default;
            })
            .Override<JValue>(static (jToken, type, env, recurse) =>
            {
                return typeof(Property).IsAssignableFrom(type) && type.IsGenericType
                    ? Activator.CreateInstance(type, recurse.TryDeserialize(type.GetGenericArguments()[0]).From(jToken, env))
                    : default;
            })
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
            .Override<JValue>(static (jToken, type, env, recurse) =>
            {
                return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)
                    ? jToken.Value is null
                        ? null
                        : recurse.TryDeserialize(type.GetGenericArguments()[0]).From(jToken, env)
                    : default;
            })
            .Override<JValue>(static (jValue, type, env, recurse) =>
            {
                if (jValue.Value is { } value)
                {
                    if (type.IsInstanceOfType(value))
                        return value;

                    if (type == typeof(int) || type == typeof(byte) || type == typeof(sbyte) || type == typeof(ushort) || type == typeof(short) || type == typeof(uint) || type == typeof(ulong) || type == typeof(long) || type == typeof(float) || type == typeof(double))
                        return Convert.ChangeType(value, type);
                }

                return null;
            })
            .Override<JObject>(static (jObject, type, env, recurse) =>
            {
                if (type.IsAssignableFrom(typeof(ExpandoObject)))
                {
                    if (recurse.TryDeserialize<JObject>().From(jObject, env) is { } processedFragment)
                    {
                        var expando = new ExpandoObject();

                        foreach (var property in processedFragment)
                            expando.TryAdd(property.Key, recurse.TryDeserialize<object>().From(property.Value, env));

                        return expando;
                    }
                }

                return default;
            })
            .Override<JObject>(static (jObject, type, env, recurse) =>
            {
                if (!type.IsSealed)
                {
                    // Elements
                    var modelTypes = env.GetCache().ModelTypesForLabels;
                    var label = jObject["label"]?.ToString();

                    var modelType = label != null && modelTypes.TryGetValue(label, out var types)
                        ? types.FirstOrDefault(type.IsAssignableFrom)
                        : default;

                    if (modelType != null && modelType != type)
                        return recurse.TryDeserialize(modelType).From(jObject, env);
                }

                return default;
            })
            .Override<JObject>(static (jObject, type, env, recurse) =>
            {
                //Vertex Properties
                var nativeTypes = env.GetCache().FastNativeTypes;

                if (nativeTypes.ContainsKey(type) || type.IsEnum && nativeTypes.ContainsKey(type.GetEnumUnderlyingType()))
                    if (jObject.TryGetValue("value", out var valueToken))
                        return recurse.TryDeserialize(type).From(valueToken, env);

                return default;
            })
            .Override<JObject>(static (jObject, type, env, recurse) =>
            {
                if (jObject.TryGetValue("@type", out var typeName) && jObject.TryGetValue("@value", out var valueToken))
                {
                    if (typeName.Type == JTokenType.String && typeName.Value<string>() is { } typeNameString && GraphSONTypes.TryGetValue(typeNameString, out var moreSpecificType))
                        if (type != moreSpecificType && type.IsAssignableFrom(moreSpecificType))
                            type = moreSpecificType;

                    return recurse.TryDeserialize(type).From(valueToken, env);
                }

                return default;
            })
            .Override<JObject>(static (jObject, type, env, recurse) =>
            {
                //@type == "g:Map"
                return jObject.TryUnmap() is { } unmappedObject
                    ? recurse.TryDeserialize(type).From(unmappedObject, env)
                    : default;
            })
            .Override<JObject>(static (jObject, type, env, recurse) =>
            {
                if (type.IsArray && !env.GetCache().FastNativeTypes.ContainsKey(type))
                {
                    var elementType = type.GetElementType()!;

                    if (jObject.TryGetValue("@type", out var typeToken) && "g:BulkSet".Equals(typeToken.Value<string>(), StringComparison.OrdinalIgnoreCase))
                    {
                        if (jObject.TryGetValue("@value", out var valueToken) && valueToken is JArray setArray)
                        {
                            var array = new ArrayList();

                            for (var i = 0; i < setArray.Count; i += 2)
                            {
                                var element = recurse.TryDeserialize(elementType).From(setArray[i], env);
                                var bulk = (int)recurse.TryDeserialize<int>().From(setArray[i + 1], env)!;

                                for (var j = 0; j < bulk; j++)
                                    array.Add(element);
                            }

                            return array.ToArray(elementType);
                        }
                    }
                }

                return default;
            })
            .Override<JObject>(static (jObject, type, env, recurse) =>
            {
                //Traversers
                if (type.IsArray && !env.GetCache().FastNativeTypes.ContainsKey(type))
                {
                    var elementType = type.GetElementType()!;

                    if (jObject.TryExpandTraverser(elementType, env, recurse) is { } enumerable)
                    {
                        var array = new ArrayList();

                        foreach (var item in enumerable)
                        {
                            array.Add(item);
                        }

                        return array.ToArray(elementType);
                    }
                }

                return default;
            })
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
