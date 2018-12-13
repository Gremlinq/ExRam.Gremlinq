using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq
{
    public static class GremlinQueryProvider
    {
        private sealed class InvalidQueryProvider : IGremlinQueryProvider
        {
            public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
            {
                return AsyncEnumerable.Throw<TElement>(new InvalidOperationException());
            }
        }

        private sealed class JsonSupportGremlinQueryProvider : IGremlinQueryProvider
        {
            private static readonly JArray EmptyJArray = new JArray();

            private readonly JsonTransformRule _baseRule;
            private readonly GraphsonDeserializer _serializer;
            private readonly IGremlinQueryProvider _baseProvider;

            public JsonSupportGremlinQueryProvider(IGremlinQueryProvider baseProvider, IGraphModel model)
            {
                _baseProvider = baseProvider;
                _serializer = new GraphsonDeserializer(model);

                _baseRule = JsonTransformRules
                    .Empty
                    .Lazy((token, recurse) =>
                    {
                        if (token is JObject jObject && jObject.Has("@type", "g:Map"))
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
                    .Lazy((token, recurse) =>
                    {
                        if (token is JObject jObject && jObject.ContainsKey("@type") && jObject["@value"] is JToken valueToken)
                        {
                            if (valueToken is JObject valueObject)
                            {
                                if (jObject.Has("@type", "g:Vertex"))
                                    valueObject.Add("type", "vertex");
                                else if (jObject.Has("@type", "g:Edge"))
                                    valueObject.Add("type", "edge");
                            }

                            return recurse(valueToken);
                        }

                        return Option<JToken>.None;
                    })
                    .Lazy((token, recurse) =>
                    {
                        if (token is JObject jObject && (jObject.Has("type", "vertex") || jObject.Has("type", "edge")) && jObject["properties"] is JObject propertiesObject)
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
            }

            public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
            {
                return _baseProvider
                    .Execute(query
                        .Cast<JToken>())
                    .Select(token => token
                        .Transform(_baseRule)
                        .IfNone(EmptyJArray))
                    .SelectMany(token => _serializer
                        .Deserialize<TElement[]>(new JTokenReader(token))
                        .ToAsyncEnumerable());
            }
        }

        public static readonly IGremlinQueryProvider Invalid = new InvalidQueryProvider();

        public static IGremlinQueryProvider WithJsonSupport(this IGremlinQueryProvider provider, IGraphModel model)
        {
            return new JsonSupportGremlinQueryProvider(provider, model);
        }
    }
}
