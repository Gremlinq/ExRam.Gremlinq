using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq
{
    internal sealed class JsonSupportGremlinQueryProvider : IGremlinQueryProvider
    {
        private readonly IGremlinQueryProvider _baseProvider;

        private static readonly ConditionalWeakTable<IGraphModel, GraphsonDeserializer> Serializers = new ConditionalWeakTable<IGraphModel, GraphsonDeserializer>();

        public JsonSupportGremlinQueryProvider(IGremlinQueryProvider baseProvider)
        {
            _baseProvider = baseProvider;
        }

        public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
        {
            var serializer = Serializers.GetValue(
                query.Model,
                model => new GraphsonDeserializer(model));

            return _baseProvider
                .Execute(query
                    .Cast<JToken>())
                .SelectMany(token => serializer
                    .Deserialize<TElement[]>(new JTokenReader(token)
                        .ToTokenEnumerable()
                        .Apply(JsonTransform
                            .Identity()
                            .GraphElements()
                            .NestedValues())
                        .ToJsonReader())
                    .ToAsyncEnumerable());
        }
    }

    internal static class GremlinQueryProvider
    {
        private sealed class InvalidQueryProvider : IGremlinQueryProvider
        {
            public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
            {
                return AsyncEnumerable.Throw<TElement>(new InvalidOperationException());
            }
        }

        public static readonly IGremlinQueryProvider Invalid = new InvalidQueryProvider();
    }
}
