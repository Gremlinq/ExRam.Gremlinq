using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExecutionResultDeserializer
    {
        private sealed class InvalidQueryExecutionResultDeserializer : IGremlinQueryExecutionResultDeserializer
        {
            public IAsyncEnumerable<TElement> Deserialize<TElement>(object result, IGremlinQueryEnvironment environment)
            {
                return AsyncEnumerableEx.Throw<TElement>(new InvalidOperationException($"{nameof(Deserialize)} must not be called on {nameof(GremlinQueryExecutionResultDeserializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQueryExecutionResultDeserializer)} on your {nameof(GremlinQuerySource)}."));
            }
        }

        private sealed class EmptyQueryExecutionResultDeserializer : IGremlinQueryExecutionResultDeserializer
        {
            public IAsyncEnumerable<TElement> Deserialize<TElement>(object result, IGremlinQueryEnvironment environment)
            {
                return AsyncEnumerable.Empty<TElement>();
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
        }

        private sealed class ToStringGremlinQueryExecutionResultDeserializer : IGremlinQueryExecutionResultDeserializer
        {
            public IAsyncEnumerable<TElement> Deserialize<TElement>(object result, IGremlinQueryEnvironment environment)
            {
                if (!typeof(TElement).IsAssignableFrom(typeof(string)))
                    throw new InvalidOperationException($"Can't deserialize a string to {typeof(TElement).Name}. Make sure you cast call Cast<string>() on the query before executing it.");

                return AsyncEnumerableEx.Return((TElement)(object)result.ToString());
            }
        }

        private sealed class DefaultGraphsonDeserializer : IGremlinQueryExecutionResultDeserializer
        {
            private readonly ConditionalWeakTable<IGremlinQueryEnvironment, GraphsonJsonSerializer>.CreateValueCallback _serializerFactory;
            private readonly ConditionalWeakTable<IGremlinQueryEnvironment, GraphsonJsonSerializer> _serializers = new ConditionalWeakTable<IGremlinQueryEnvironment, GraphsonJsonSerializer>();

            public DefaultGraphsonDeserializer(params IJTokenConverter[] additionalConverters)
            {
                _serializerFactory = env => new GraphsonJsonSerializer(env, additionalConverters);
            }

            public IAsyncEnumerable<TElement> Deserialize<TElement>(object executionResult, IGremlinQueryEnvironment environment)
            {
                if (executionResult is JToken jToken)
                {
                    var baseDeserializer = _serializers.GetValue(
                        environment,
                        _serializerFactory);

                    try
                    {
                        var transformed = Transform(jToken);

                        return baseDeserializer
                            .Deserialize<TElement[]>(new JTokenReader(transformed))
                            .ToAsyncEnumerable();
                    }
                    catch (JsonReaderException ex)
                    {
                        throw new GraphsonMappingException($"Error mapping\r\n\r\n{jToken}\r\n\r\nto an object of type {typeof(TElement[])}.", ex);
                    }
                }

                throw new ArgumentException($"Cannot handle execution results of type {executionResult.GetType()}.");
            }

            private static JToken Transform(JToken jToken)
            {
                switch (jToken)
                {
                    case JObject jObject:
                    {
                        foreach (var property in jObject)
                        {
                            jObject[property.Key] = Transform(property.Value);
                        }

                        if (jObject.TryGetValue("@type", out var nestedType))
                        {
                            if ("g:Map".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase))
                            {
                                if (jObject.TryGetValue("@value", out var value) && value is JArray mapArray)
                                {
                                    var retObject = new JObject();

                                    for (var i = 0; i < mapArray.Count / 2; i++)
                                    {
                                        retObject.Add(mapArray[i * 2].Value<string>(), Transform(mapArray[i * 2 + 1]));
                                    }

                                    return retObject;
                                }
                            }
                            else if ("g:Traverser".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase))
                                return jObject;
                            else if (jObject.TryGetValue("@value", out var value))
                                return Transform(value);
                        }

                        break;
                    }
                    case JArray jArray:
                    {
                        for(var i = 0; i < jArray.Count; i++)
                        {
                            jArray[i] = Transform(jArray[i]);
                        }

                        return jArray;
                    }
                }

                return jToken;
            }
        }

        public static readonly IGremlinQueryExecutionResultDeserializer ToGraphson = new ToGraphsonGremlinQueryExecutionResultDeserializer();

        public static new readonly IGremlinQueryExecutionResultDeserializer ToString = new ToStringGremlinQueryExecutionResultDeserializer();

        public static readonly IGremlinQueryExecutionResultDeserializer Invalid = new InvalidQueryExecutionResultDeserializer();

        public static readonly IGremlinQueryExecutionResultDeserializer Empty = new EmptyQueryExecutionResultDeserializer();

        public static readonly IGremlinQueryExecutionResultDeserializer Graphson = new DefaultGraphsonDeserializer();

        public static IGremlinQueryExecutionResultDeserializer GraphsonWithJsonConverters(params IJTokenConverter[] additionalConverters) => new DefaultGraphsonDeserializer(additionalConverters);
    }
}
