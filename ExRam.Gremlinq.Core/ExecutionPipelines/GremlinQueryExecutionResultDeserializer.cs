using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Providers;
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

        private sealed class ToStringGremlinQueryExecutionResultDeserializer : IGremlinQueryExecutionResultDeserializer
        {
            public IAsyncEnumerable<TElement> Deserialize<TElement>(object result, IGremlinQueryEnvironment environment)
            {
                if (!typeof(TElement).IsAssignableFrom(typeof(string)))
                    throw new InvalidOperationException($"Can't deserialize a string to {typeof(TElement).Name}. Make sure you cast call Cast<string>() on the query before executing it.");

                return AsyncEnumerableEx.Return((TElement)(object)result?.ToString());
            }
        }

        private sealed class DefaultGraphsonDeserializer : IGremlinQueryExecutionResultDeserializer
        {
            private readonly ConditionalWeakTable<IGremlinQueryEnvironment, GraphsonJsonSerializer> _serializers = new ConditionalWeakTable<IGremlinQueryEnvironment, GraphsonJsonSerializer>();

            private readonly JsonConverter[] _additionalConverters;

            public DefaultGraphsonDeserializer(params JsonConverter[] additionalConverters)
            {
                _additionalConverters = additionalConverters;
            }

            public IAsyncEnumerable<TElement> Deserialize<TElement>(object executionResult, IGremlinQueryEnvironment environment)
            {
                if (executionResult is JToken jToken)
                {
                    var baseDeserializer = _serializers.GetValue(
                        environment,
                        gremlinQueryEnvironment => new GraphsonJsonSerializer(gremlinQueryEnvironment, _additionalConverters));

                    try
                    {
                        return baseDeserializer
                            .Deserialize<TElement[]>(new JTokenReader(jToken)
                                .ToTokenEnumerable()
                                .Apply(JsonTransform
                                    .Identity()
                                    .GraphElements()
                                    .NestedValues())
                                .ToJsonReader())
                            .ToAsyncEnumerable();
                    }
                    catch (JsonReaderException ex)
                    {
                        throw new GraphsonMappingException($"Error mapping\r\n\r\n{jToken}\r\n\r\nto an object of type {typeof(TElement[])}.", ex);
                    }
                }

                throw new ArgumentException($"Cannot handle execution results of type {executionResult.GetType()}.");
            }
        }

        public new static readonly IGremlinQueryExecutionResultDeserializer ToString = new ToStringGremlinQueryExecutionResultDeserializer();

        public static readonly IGremlinQueryExecutionResultDeserializer Invalid = new InvalidQueryExecutionResultDeserializer();

        public static readonly IGremlinQueryExecutionResultDeserializer Graphson = new DefaultGraphsonDeserializer();

        public static IGremlinQueryExecutionResultDeserializer GraphsonWithJsonConverters(params JsonConverter[] additionalConverters) => new DefaultGraphsonDeserializer(additionalConverters);

    }
}
