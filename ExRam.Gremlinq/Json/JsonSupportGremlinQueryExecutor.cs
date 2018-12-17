using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq
{
    internal sealed class JsonSupportGremlinQueryExecutor : IGremlinQueryExecutor
    {
        private readonly IGremlinQueryExecutor _baseExecutor;

        private static readonly ConditionalWeakTable<IGraphModel, GraphsonDeserializer> Serializers = new ConditionalWeakTable<IGraphModel, GraphsonDeserializer>();

        public JsonSupportGremlinQueryExecutor(IGremlinQueryExecutor baseExecutor)
        {
            _baseExecutor = baseExecutor;
        }

        public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
        {
            var serializer = Serializers.GetValue(
                query.Model,
                model => new GraphsonDeserializer(model));

            return _baseExecutor
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

        public bool SupportsElementType(Type type)
        {
            return _baseExecutor.SupportsElementType(typeof(JToken));
        }
    }
}
