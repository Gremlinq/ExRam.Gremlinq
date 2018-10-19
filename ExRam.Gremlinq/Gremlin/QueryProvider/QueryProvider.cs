using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq
{
    public static class QueryProvider
    {
        private sealed class JsonSupportGremlinQueryProvider : IGremlinQueryProvider
        {
            private static readonly JArray EmptyJArray = new JArray();
            private static readonly GraphsonDeserializer Serializer = new GraphsonDeserializer();

            private readonly IGraphModel _model;
            private readonly IGremlinQueryProvider _baseProvider;

            public JsonSupportGremlinQueryProvider(IGremlinQueryProvider baseProvider, IGraphModel model)
            {
                _model = model;
                _baseProvider = baseProvider;
            }

            public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
            {
                var transformRule = JsonTransformRules
                    .Empty
                    .Lazy((token, recurse) => token is JObject jObject && (jObject["@type"]?.ToString().Equals("g:Property", StringComparison.OrdinalIgnoreCase)).GetValueOrDefault()
                        ? jObject.TryGetValue("@value")
                            .Map(value => value as JObject)
                            .Bind(valueObject => valueObject.TryGetValue("value"))
                        : Option<JToken>.None)
                    .Lazy((token, recurse) =>
                    {
                        if (token is JObject jObject && (jObject["@type"]?.ToString().Equals("g:Map", StringComparison.OrdinalIgnoreCase)).GetValueOrDefault())
                        {
                            return jObject.TryGetValue("@value")
                                .Map(value => value as JArray)
                                .Bind(array =>
                                {
                                    var mapObject = new JObject();

                                    for (var i = 0; i < array.Count - 1; i += 2)
                                    {
                                        if (array[i] is JValue value && value.Value is string key)
                                            mapObject[key] = array[i + 1];
                                    }

                                    return recurse(mapObject);
                                });
                        }

                        return Option<JToken>.None;
                    })
                    .Lazy((token, recurse) => token is JObject jObject && jObject.ContainsKey("@type") && jObject["@value"] is JToken valueToken
                        ? recurse(valueToken)
                        : Option<JToken>.None)
                    .Lazy((token, recurse) =>
                    {
                        if (token is JObject jObject && (jObject.Parent?.Parent is JObject typedVertexProperty && typedVertexProperty["@type"]?.ToString() == "g:VertexProperty" || jObject.Parent?.Parent?.Parent?.Parent is JProperty parentProperty && parentProperty.Name.Equals("properties", StringComparison.OrdinalIgnoreCase)))
                        {
                            return jObject
                                .TryGetValue("value")
                                .Bind(recurse);
                        }

                        return Option<JToken>.None;
                    })
                    .Lazy((token, recurse) =>
                    {
                        if (token is JObject jObject && jObject.ContainsKey("label") && !jObject.ContainsKey("$type"))
                        {
                            return _model
                                .TryGetElementTypeOfLabel(jObject["label"].ToString())
                                .Bind(type =>
                                {
                                    jObject.AddFirst(new JProperty("$type", type.AssemblyQualifiedName));

                                    return recurse(jObject);
                                });
                        }

                        return Option<JToken>.None;
                    })
                    .Lazy((token, recurse) =>
                    {
                        if (token is JObject jObject && jObject.ContainsKey("label") && jObject["properties"] is JObject propertiesObject)
                        {
                            foreach (var item in propertiesObject)
                            {
                                jObject[item.Key] = recurse(item.Value).IfNone(jObject[item.Key]);
                            }

                            propertiesObject.Parent.Remove();

                            return recurse(jObject);
                        }

                        return Option<JToken>.None;
                    })
                    .Lazy(JsonTransformRules.Identity);

                return _baseProvider
                    .Execute(query
                        .Resolve(_model)
                        .Cast<JToken>())
                    .Select(token => token
                        .Transform(transformRule)
                        .IfNone(EmptyJArray))
                    .Select(token => token is JArray ? token : new JArray(token))
                    .SelectMany(token => Serializer
                        .Deserialize<TElement[]>(new JTokenReader(token))
                        .ToAsyncEnumerable());
            }
        }

        public static IAsyncEnumerable<TElement> Execute<TElement>(this IGremlinQuery<TElement> query)
        {
            var queryProvider = query
                .TryGetTypedGremlinQueryProvider()
                .IfNone(() => throw new ArgumentException("Could not find an instance of IGremlinQueryProvider in the query"));

            return queryProvider.Execute(query);
        }

        public static IGremlinQueryProvider WithJsonSupport(this IGremlinQueryProvider provider, IGraphModel model)
        {
            return new JsonSupportGremlinQueryProvider(provider, model);
        }
    }
}
