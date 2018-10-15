using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq
{
    public static class GremlinQueryProvider
    {
        private sealed class JsonSupportTypedGremlinQueryProvider : ITypedGremlinQueryProvider
        {
            private static readonly JArray EmptyJArray = new JArray();
            private static readonly GraphsonDeserializer Serializer = new GraphsonDeserializer();

            private readonly IGraphModel _model;
            private readonly ITypedGremlinQueryProvider _baseProvider;

            public JsonSupportTypedGremlinQueryProvider(ITypedGremlinQueryProvider baseProvider, IGraphModel model)
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
                        if (token is JObject jObject && !jObject.ContainsKey("$type"))
                        {
                            return jObject
                                .TryGetValue("label")
                                .Bind(labelToken => _model.TryGetElementTypeOfLabel(labelToken.ToString()))
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
                        if (token is JObject jObject && jObject["properties"] is JObject propertiesObject)
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
       
        //private sealed class SelectNativeGremlinQueryProvider<TNativeSource, TNativeTarget> : ITypedGremlinQueryProvider
        //{
        //    private readonly Func<TNativeSource, TNativeTarget> _projection;
        //    private readonly ITypedGremlinQueryProvider _provider;

        //    public SelectNativeGremlinQueryProvider(ITypedGremlinQueryProvider provider, Func<TNativeSource, TNativeTarget> projection)
        //    {
        //        _provider = provider;
        //        _projection = projection;
        //    }

        //    public IAsyncEnumerable<TNativeTarget> Execute(string query, IDictionary<string, object> parameters)
        //    {

        //    }

        //    public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
        //    {
        //        return _provider
        //            .Execute(query.Cast<TNativeSource>())
        //            .Select(_projection);
        //    }
        //}

        public static IAsyncEnumerable<TElement> Execute<TElement>(this IGremlinQuery<TElement> query)
        {
            var queryProvider = query
                .TryGetTypedGremlinQueryProvider()
                .IfNone(() => throw new ArgumentException("Could not find an instance of ITypedGremlinQueryProvider in the query"));

            return queryProvider.Execute(query);
        }

        public static ITypedGremlinQueryProvider WithJsonSupport(this ITypedGremlinQueryProvider provider, IGraphModel model)
        {
            return new JsonSupportTypedGremlinQueryProvider(provider, model);
        }
       
        //public static INativeGremlinQueryProvider<TNativeTarget> Select<TNativeSource, TNativeTarget>(this INativeGremlinQueryProvider<TNativeSource> provider, Func<TNativeSource, TNativeTarget> projection)
        //{
        //    return new SelectNativeGremlinQueryProvider<TNativeSource, TNativeTarget>(provider, projection);
        //}
    }
}
