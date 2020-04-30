using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using ExRam.Gremlinq.Core.GraphElements;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            private readonly IQueryFragmentDeserializer _fragmentSerializer;

            public GremlinQueryExecutionResultDeserializerImpl(IQueryFragmentDeserializer fragmentSerializer)
            {
                _fragmentSerializer = fragmentSerializer;
            }

            public IAsyncEnumerable<TElement> Deserialize<TElement>(object executionResult, IGremlinQueryEnvironment environment)
            {
                var result = _fragmentSerializer
                    .TryDeserialize(executionResult, typeof(TElement[]), environment);

                if (result is IAsyncEnumerable<TElement> enumerable)
                    return enumerable;

                if (result is TElement[] elements)
                    return elements.ToAsyncEnumerable();

                if (result is TElement element)
                    return AsyncEnumerableEx.Return(element);

                throw new NotImplementedException();
            }

            public IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IQueryFragmentDeserializer, IQueryFragmentDeserializer> transformation)
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

            public IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IQueryFragmentDeserializer, IQueryFragmentDeserializer> transformation)
            {
                throw new NotSupportedException();
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

            public IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IQueryFragmentDeserializer, IQueryFragmentDeserializer> transformation)
            {
                throw new NotSupportedException();
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

            public IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IQueryFragmentDeserializer, IQueryFragmentDeserializer> transformation)
            {
                throw new NotSupportedException();
            }
        }

        private sealed class EmptyQueryExecutionResultDeserializer : IGremlinQueryExecutionResultDeserializer
        {
            public IAsyncEnumerable<TElement> Deserialize<TElement>(object result, IGremlinQueryEnvironment environment)
            {
                return AsyncEnumerable.Empty<TElement>();
            }

            public IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IQueryFragmentDeserializer, IQueryFragmentDeserializer> transformation)
            {
                throw new NotSupportedException();
            }
        }

        private sealed class FromJTokenGremlinQueryExecutionResultDeserializer : IGremlinQueryExecutionResultDeserializer
        {
            private readonly IQueryFragmentDeserializer _fragmentDeserializer;

            public FromJTokenGremlinQueryExecutionResultDeserializer(IQueryFragmentDeserializer fragmentDeserializer)
            {
                _fragmentDeserializer = fragmentDeserializer;
            }

            public IAsyncEnumerable<TElement> Deserialize<TElement>(object executionResult, IGremlinQueryEnvironment environment)
            {
                if (executionResult is JToken jToken)
                {
                    try
                    {
                        return ((TElement[])_fragmentDeserializer.TryDeserialize(jToken, typeof(TElement[]), environment))
                            .ToAsyncEnumerable();
                    }
                    catch (JsonReaderException ex)
                    {
                        throw new GraphsonMappingException($"Error mapping\r\n\r\n{jToken}\r\n\r\nto an object of type {typeof(TElement[])}.", ex);
                    }
                }

                throw new ArgumentException($"Cannot handle execution results of type {executionResult.GetType()}.");
            }

            public IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IQueryFragmentDeserializer, IQueryFragmentDeserializer> transformation)
            {
                return new FromJTokenGremlinQueryExecutionResultDeserializer(transformation(_fragmentDeserializer));
            }
        }

        public static readonly IGremlinQueryExecutionResultDeserializer Invalid = new InvalidQueryExecutionResultDeserializer();

        public static readonly IGremlinQueryExecutionResultDeserializer Empty = new EmptyQueryExecutionResultDeserializer();

        public static readonly IGremlinQueryExecutionResultDeserializer FromJToken = new FromJTokenGremlinQueryExecutionResultDeserializer(QueryFragmentDeserializer
            .Identity
            .Override<JToken>((jToken, type, env, overridden, recurse) =>
            {
                var ret = jToken.ToObject(
                    type,
                    new GraphsonJsonSerializer(
                        DefaultValueHandling.Populate,
                        env,
                        recurse));

                if (!(ret is JToken) && jToken is JObject element)
                {
                    if (element.ContainsKey("id") && element.TryGetValue("label", out var label) && label.Type == JTokenType.String && element["properties"] is { } propertiesToken)
                    {
                        if (propertiesToken.TryUnmap() is { } jObject)
                            propertiesToken = jObject;

                        new GraphsonJsonSerializer(
                            DefaultValueHandling.Ignore,
                            env,
                            recurse).Populate(new JTokenReader(propertiesToken), ret);
                    }
                }

                return ret;
            })
            .Override<JToken>((jToken, type, env, overridden, recurse) =>
            {
                if (type == typeof(TimeSpan))
                {
                    if (recurse.TryDeserialize(jToken, typeof(string), env) is string strValue)
                    {
                        return XmlConvert.ToTimeSpan(strValue);
                    }
                }

                return overridden(jToken);
            })
            .Override<JToken>((jToken, type, env, overridden, recurse) =>
            {
                if (type == typeof(DateTimeOffset))
                {
                    if (jToken is JValue jValue)
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
                                    return DateTimeOffset.FromUnixTimeMilliseconds(jValue.ToObject<long>());

                                break;
                            }
                        }
                    }
                }

                return overridden(jToken);
            })
            .Override<JToken>((jToken, type, env, overridden, recurse) =>
            {
                if (type == typeof(DateTime))
                {
                    if (jToken is JValue jValue)
                    {
                        switch (jValue.Value)
                        {
                            case DateTime dateTime:
                                return dateTime;
                            case DateTimeOffset dateTimeOffset:
                                return dateTimeOffset.UtcDateTime;
                        }

                        if (jValue.Type == JTokenType.Integer)
                        {
                            return new DateTime(DateTimeOffset.FromUnixTimeMilliseconds(jValue.ToObject<long>()).Ticks, DateTimeKind.Utc);
                        }
                    }
                }

                return overridden(jToken);

            })
            .Override<JToken>((jToken, type, env, overridden, recurse) =>
            {
                if (jToken is JObject jObject)
                {
                    if (jObject.ContainsKey("@type") && jObject.TryGetValue("@value", out var valueToken))
                        return recurse.TryDeserialize(valueToken, type, env);
                }

                return overridden(jToken);
            })
            .Override<JToken>((jToken, type, env, overridden, recurse) =>
            {
                if (!type.IsArray)
                {
                    if (!type.IsInstanceOfType(jToken))
                    {
                        var itemType = default(Type);

                        if (type.IsGenericType)
                        {
                            var genericTypeDefinition = type.GetGenericTypeDefinition();
                            if (genericTypeDefinition == typeof(Nullable<>))
                                itemType = type.GetGenericArguments()[0];
                        }

                        if (jToken is JArray array)
                        {
                            if (array.Count != 1)
                            {
                                if (array.Count == 0 && (type.IsClass || itemType != null))
                                {
                                    return default(object);
                                }

                                throw new JsonReaderException($"Cannot convert array\r\n\r\n{array}\r\n\r\nto scalar value of type {type}.");
                            }

                            return recurse.TryDeserialize(array[0], itemType ?? type, env);
                        }

                        if (jToken is JValue jValue && jValue.Value == null && itemType != null)
                            return null;
                    }
                }

                return overridden(jToken);
            })
            .Override<JToken>((jToken, type, env, overridden, recurse) =>
            {
                if (jToken.TryUnmap() is { } jObject)
                    return recurse.TryDeserialize<JToken>(jObject, type, env);

                return overridden(jToken);
            })
            .Override<JToken>((jToken, type, env, overridden, recurse) =>
            {
                if (type.IsArray && jToken is JArray jArray)
                {
                    var elementType = type.GetElementType();
                    var array = new ArrayList(jArray.Count);

                    foreach (var jArrayItem in jArray)
                    {
                        var bulk = 1;
                        var effectiveArrayItem = jArrayItem;

                        if (jArrayItem is JObject traverserObject && traverserObject.TryGetValue("@type", out var nestedType) && "g:Traverser".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase) && traverserObject.TryGetValue("@value", out var valueToken) && valueToken is JObject nestedTraverserObject)
                        {
                            if (nestedTraverserObject.TryGetValue("bulk", out var bulkToken))
                            {
                                if (recurse.TryDeserialize(bulkToken, typeof(int), env) is int bulkObject)
                                    bulk = bulkObject;
                            }

                            if (nestedTraverserObject.TryGetValue("value", out var traverserValue))
                            {
                                effectiveArrayItem = traverserValue;
                            }
                        }

                        if (recurse.TryDeserialize(effectiveArrayItem, elementType, env) is { } item)
                        {
                            for (var i = 0; i < bulk; i++)
                            {
                                array.Add(item);
                            }
                        }
                    }

                    return array.ToArray(elementType);
                }

                return overridden(jToken);
            })
            .Override<JToken>((jToken, type, env, overridden, recurse) =>
            {
                if (jToken is JObject)
                {
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


                    var label = jToken["label"]?.ToString();

                    var modelType = label != null && modelTypes.TryGetValue(label, out var types)
                        ? types.FirstOrDefault(type => type.IsAssignableFrom(type))
                        : default;

                    if (modelType == null)
                    {
                        if (type == typeof(IVertex))
                            modelType = typeof(VertexImpl);
                        else if (type == typeof(IEdge))
                            modelType = typeof(EdgeImpl);
                    }

                    if (modelType != null && modelType != type)
                        return recurse.TryDeserialize(jToken, modelType, env);
                }

                return overridden(jToken);
            })
            .Override<JToken>((jToken, type, env, overridden, recurse) =>
            {
                var nativeTypes = env.Model.NativeTypes;

                if (nativeTypes.Contains(type) || (type.IsEnum && nativeTypes.Contains(type.GetEnumUnderlyingType())))
                {
                    if (jToken is JObject jObject && jObject.ContainsKey("value"))
                        return recurse.TryDeserialize(jObject["value"], type, env);
                }

                return overridden(jToken);
            }));

        public static readonly IGremlinQueryExecutionResultDeserializer Identity = new GremlinQueryExecutionResultDeserializerImpl(QueryFragmentDeserializer.Identity);

        public static readonly IGremlinQueryExecutionResultDeserializer ToGraphsonString = new ToGraphsonGremlinQueryExecutionResultDeserializer();

        public static new readonly IGremlinQueryExecutionResultDeserializer ToString = new ToStringGremlinQueryExecutionResultDeserializer();
    }
}
