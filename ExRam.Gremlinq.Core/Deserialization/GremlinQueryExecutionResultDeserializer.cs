using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using ExRam.Gremlinq.Core.GraphElements;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExecutionResultDeserializer
    {
        private static readonly ConditionalWeakTable<IGraphModel, IDictionary<string, Type[]>> ModelTypes = new ConditionalWeakTable<IGraphModel, IDictionary<string, Type[]>>();

        private sealed class VertexImpl : IVertex
        {
            public object? Id { get; set; }
        }

        private sealed class EdgeImpl : IEdge
        {
            public object? Id { get; set; }
        }

        private sealed class GremlinQueryExecutionResultDeserializerImpl : IGremlinQueryExecutionResultDeserializer
        {
            private readonly IGremlinQueryFragmentDeserializer _fragmentSerializer;

            public GremlinQueryExecutionResultDeserializerImpl(IGremlinQueryFragmentDeserializer fragmentSerializer)
            {
                _fragmentSerializer = fragmentSerializer;
            }

            public IAsyncEnumerable<TElement> Deserialize<TElement>(object executionResult, IGremlinQueryEnvironment environment)
            {
                var result = _fragmentSerializer
                    .TryDeserialize(executionResult, typeof(TElement[]), environment);

                return result switch
                {
                    TElement[] elements => elements.ToAsyncEnumerable(),
                    IAsyncEnumerable<TElement> enumerable => enumerable,
                    TElement element => AsyncEnumerableEx.Return(element),
                    IEnumerable enumerable => enumerable.Cast<TElement>().ToAsyncEnumerable(),
                    { } obj => throw new InvalidCastException($"A result of type {obj.GetType()} can't be interpreted as {nameof(IAsyncEnumerable<TElement>)}."),
                    _ => AsyncEnumerable.Empty<TElement>()
                };
            }

            public IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IGremlinQueryFragmentDeserializer, IGremlinQueryFragmentDeserializer> transformation)
            {
                return new GremlinQueryExecutionResultDeserializerImpl(transformation(_fragmentSerializer));
            }
        }

        private sealed class InvalidQueryExecutionResultDeserializer : IGremlinQueryExecutionResultDeserializer
        {
            public IAsyncEnumerable<TElement> Deserialize<TElement>(object result, IGremlinQueryEnvironment environment)
            {
                return AsyncEnumerableEx.Throw<TElement>(new InvalidOperationException($"{nameof(Deserialize)} must not be called on {nameof(GremlinQueryExecutionResultDeserializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQueryExecutionResultDeserializer)} on your {nameof(GremlinQuerySource)}."));
            }

            public IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IGremlinQueryFragmentDeserializer, IGremlinQueryFragmentDeserializer> transformation)
            {
                throw new InvalidOperationException($"{nameof(ConfigureFragmentDeserializer)} cannot be called on {nameof(GremlinQueryExecutionResultDeserializer)}.{nameof(Invalid)}.");
            }
        }

        private sealed class ToStringGremlinQueryExecutionResultDeserializer : IGremlinQueryExecutionResultDeserializer
        {
            public IAsyncEnumerable<TElement> Deserialize<TElement>(object result, IGremlinQueryEnvironment environment)
            {
                if (!typeof(TElement).IsAssignableFrom(typeof(string)))
                    throw new InvalidOperationException($"Can't deserialize a string to {typeof(TElement).Name}. Make sure you cast call Cast<string>() on the query before executing it.");

                return AsyncEnumerableEx.Return((TElement)(object)result.ToString());
            }

            public IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IGremlinQueryFragmentDeserializer, IGremlinQueryFragmentDeserializer> transformation)
            {
                throw new InvalidOperationException($"{nameof(ConfigureFragmentDeserializer)} cannot be called on {nameof(GremlinQueryExecutionResultDeserializer)}.{nameof(GremlinQueryExecutionResultDeserializer.ToString)}.");
            }
        }

        private sealed class ToGraphsonGremlinQueryExecutionResultDeserializer : IGremlinQueryExecutionResultDeserializer
        {
            public IAsyncEnumerable<TElement> Deserialize<TElement>(object result, IGremlinQueryEnvironment environment)
            {
                if (!typeof(TElement).IsAssignableFrom(typeof(string)))
                    throw new InvalidOperationException($"Can't deserialize a string to {typeof(TElement).Name}. Make sure you cast call Cast<string>() on the query before executing it.");

                return AsyncEnumerableEx.Return((TElement)(object)new GraphSON2Writer().WriteObject(result));
            }

            public IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IGremlinQueryFragmentDeserializer, IGremlinQueryFragmentDeserializer> transformation)
            {
                throw new InvalidOperationException($"{nameof(ConfigureFragmentDeserializer)} cannot be called on {nameof(GremlinQueryExecutionResultDeserializer)}.{nameof(ToGraphsonString)}.");
            }
        }

        public static readonly IGremlinQueryExecutionResultDeserializer Identity = new GremlinQueryExecutionResultDeserializerImpl(GremlinQueryFragmentDeserializer.Identity);

        public static readonly IGremlinQueryExecutionResultDeserializer Invalid = new InvalidQueryExecutionResultDeserializer();

        public static readonly IGremlinQueryExecutionResultDeserializer ToGraphsonString = new ToGraphsonGremlinQueryExecutionResultDeserializer();

        public static new readonly IGremlinQueryExecutionResultDeserializer ToString = new ToStringGremlinQueryExecutionResultDeserializer();

        // ReSharper disable ConvertToLambdaExpression
        public static readonly IGremlinQueryExecutionResultDeserializer FromJToken = new GremlinQueryExecutionResultDeserializerImpl(GremlinQueryFragmentDeserializer
            .Identity
            .Override<JToken>((jToken, type, env, overridden, recurse) =>
            {
                var populatingSerializer = env
                    .GetCache()
                    .GetPopulatingJsonSerializer(recurse);

                if (type == typeof(object))
                    return jToken.ToObject<IDictionary<string, object>>(populatingSerializer);

                var ret = jToken.ToObject(type, populatingSerializer);

                if (!(ret is JToken) && jToken is JObject element)
                {
                    var ignoringSerializer = env
                        .GetCache()
                        .GetIgnoringJsonSerializer(recurse);

                    if (element.TryGetElementProperties() is { } propertiesToken)
                        ignoringSerializer.Populate(new JTokenReader(propertiesToken), ret);
                }

                return ret;
            })
            .Override<JToken>((jToken, type, env, overridden, recurse) =>
            {
                if (type.IsArray && !env.Model.NativeTypes.Contains(type))
                {
                    type = type.GetElementType();

                    var array = Array.CreateInstance(type, 1);
                    array.SetValue(recurse.TryDeserialize(jToken, type, env), 0);

                    return array;
                }

                return overridden(jToken, type, env, recurse);
            })
            .Override<JToken>((jToken, type, env, overridden, recurse) =>
            {
                return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)
                    ? jToken.Type == JTokenType.Null
                        ? null
                        : recurse.TryDeserialize(jToken, type.GetGenericArguments()[0], env)
                    : overridden(jToken, type, env, recurse);
            })
            .Override<JValue>((jToken, type, env, overridden, recurse) =>
            {
                return jToken.ToObject(type);
            })
            .Override<JValue>((jToken, type, env, overridden, recurse) =>
            {
                return typeof(Property).IsAssignableFrom(type) && type.IsGenericType
                    ? Activator.CreateInstance(type, recurse.TryDeserialize(jToken, type.GetGenericArguments()[0], env))
                    : overridden(jToken, type, env, recurse);
            })
            .Override<JValue>((jValue, type, env, overridden, recurse) =>
            {
                return type == typeof(TimeSpan)
                    ? XmlConvert.ToTimeSpan(jValue.Value<string>())
                    : overridden(jValue, type, env, recurse);
            })
            .Override<JValue>((jValue, type, env, overridden, recurse) =>
            {
                if (type == typeof(DateTimeOffset))
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
                }

                return overridden(jValue, type, env, recurse);
            })
            .Override<JValue>((jValue, type, env, overridden, recurse) =>
            {
                if (type == typeof(DateTime))
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
                }

                return overridden(jValue, type, env, recurse);
            })
            .Override<JValue>((jValue, type, env, overridden, recurse) =>
            {
                return type == typeof(byte[]) && jValue.Type == JTokenType.String
                    ? Convert.FromBase64String(jValue.Value<string>())
                    : overridden(jValue, type, env, recurse);
            })
            .Override<JValue>((jToken, type, env, overridden, recurse) =>
            {
                return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)
                    ? jToken.Value is null
                        ? null
                        : recurse.TryDeserialize(jToken, type.GetGenericArguments()[0], env)
                    : overridden(jToken, type, env, recurse);
            })
            .Override<JObject>((jObject, type, env, overridden, recurse) =>
            {
                // Elements
                var modelTypes = ModelTypes.GetValue(
                    env.Model,
                    closureModel =>
                    {
                        return closureModel
                            .VerticesModel
                            .Metadata
                            .Concat(closureModel.EdgesModel.Metadata)
                            .GroupBy(x => x.Value.Label)
                            .ToDictionary(
                                group => group.Key,
                                group => group
                                    .Select(x => x.Key)
                                    .ToArray(),
                                StringComparer.OrdinalIgnoreCase);
                    });


                var label = jObject["label"]?.ToString();

                var modelType = label != null && modelTypes.TryGetValue(label, out var types)
                    ? types.FirstOrDefault(possibleType => type.IsAssignableFrom(possibleType))
                    : default;

                if (modelType == null)
                {
                    if (type == typeof(IVertex))
                        modelType = typeof(VertexImpl);
                    else if (type == typeof(IEdge))
                        modelType = typeof(EdgeImpl);
                }

                if (modelType != null && modelType != type)
                    return recurse.TryDeserialize(jObject, modelType, env);

                return overridden(jObject, type, env, recurse);
            })
            .Override<JObject>((jObject, type, env, overridden, recurse) =>
            {
                //Vertex Properties
                var nativeTypes = env.Model.NativeTypes;

                if (nativeTypes.Contains(type) || (type.IsEnum && nativeTypes.Contains(type.GetEnumUnderlyingType())))
                {
                    if (jObject.TryGetValue("value", out var valueToken))
                        return recurse.TryDeserialize(valueToken, type, env);
                }

                return overridden(jObject, type, env, recurse);
            })
            .Override<JObject>((jObject, type, env, overridden, recurse) =>
            {
                if (jObject.ContainsKey("@type") && jObject.TryGetValue("@value", out var valueToken))
                    return recurse.TryDeserialize(valueToken, type, env);

                return overridden(jObject, type, env, recurse);
            })
            .Override<JObject>((jObject, type, env, overridden, recurse) =>
            {
                //@type == "g:Map"
                return jObject.TryUnmap() is { } unmappedObject
                    ? recurse.TryDeserialize(unmappedObject, type, env)
                    : overridden(jObject, type, env, recurse);
            })
            .Override<JObject>((jObject, type, env, overridden, recurse) =>
            {
                if (type.IsArray && !env.Model.NativeTypes.Contains(type))
                {
                    var elementType = type.GetElementType();

                    if (jObject.TryGetValue("@type", out var typeToken) && "g:BulkSet".Equals(typeToken.Value<string>(), StringComparison.OrdinalIgnoreCase))
                    {
                        if (jObject.TryGetValue("@value", out var valueToken) && valueToken is JArray setArray)
                        {
                            var array = new ArrayList();

                            for (var i = 0; i < setArray.Count; i += 2)
                            {
                                var element = recurse.TryDeserialize(setArray[i], elementType, env);
                                var bulk = (int)recurse.TryDeserialize(setArray[i + 1], typeof(int), env)!;

                                for (var j = 0; j < bulk; j++)
                                {
                                    array.Add(element);
                                }
                            }

                            return array.ToArray(elementType);
                        }
                    }
                }

                return overridden(jObject, type, env, recurse);
            })
            .Override<JArray>((jArray, type, env, overridden, recurse) =>
            {
                if ((!type.IsArray || env.Model.NativeTypes.Contains(type)) && !type.IsInstanceOfType(jArray))
                {
                    return jArray.Count != 1
                        ? jArray.Count == 0 && type.IsClass
                            ? (object?)default
                            : throw new JsonReaderException($"Cannot convert array\r\n\r\n{jArray}\r\n\r\nto scalar value of type {type}.")
                        : recurse.TryDeserialize(jArray[0], type, env);
                }

                return overridden(jArray, type, env, recurse);
            })
            .Override<JArray>((jArray, type, env, overridden, recurse) =>
            {
                return type.IsAssignableFrom(typeof(object[])) && recurse.TryDeserialize(jArray, typeof(object[]), env) is object[] tokens
                    ? tokens
                    : overridden(jArray, type, env, recurse);
            })
            .Override<JArray>((jArray, type, env, overridden, recurse) =>
            {
                //Traversers
                if (!type.IsArray || env.Model.NativeTypes.Contains(type))
                    return overridden(jArray, type, env, recurse);

                var array = default(ArrayList);
                var elementType = type.GetElementType();

                for (var i = 0; i < jArray.Count; i++)
                {
                    var bulk = 1;
                    var effectiveArrayItem = jArray[i];

                    if (effectiveArrayItem is JObject traverserObject && traverserObject.TryGetValue("@type", out var nestedType) && "g:Traverser".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase) && traverserObject.TryGetValue("@value", out var valueToken) && valueToken is JObject nestedTraverserObject)
                    {
                        if (nestedTraverserObject.TryGetValue("bulk", out var bulkToken) && recurse.TryDeserialize(bulkToken, typeof(int), env) is int bulkObject)
                            bulk = bulkObject;

                        if (nestedTraverserObject.TryGetValue("value", out var traverserValue))
                            effectiveArrayItem = traverserValue;
                    }

                    if (recurse.TryDeserialize(effectiveArrayItem, elementType, env) is { } item)
                    {
                        if (jArray.Count == 1 && bulk == 1)
                        {
                            var ret = Array.CreateInstance(elementType, 1);
                            ret.SetValue(item, 0);

                            return ret;
                        }

                        array ??= new ArrayList(jArray.Count);

                        for (var j = 0; j < bulk; j++)
                        {
                            array.Add(item);
                        }
                    }
                }

                return array?.ToArray(elementType) ?? Array.CreateInstance(elementType, 0);
            }));
            // ReSharper restore ConvertToLambdaExpression
    }
}
