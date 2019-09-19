using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers
{
    public sealed class DefaultGraphsonDeserializer : IGremlinQueryExecutionResultDeserializer
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
}
