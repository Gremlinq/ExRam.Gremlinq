using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using LanguageExt;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq
{
    public static class GremlinQueryProvider
    {
        private abstract class GremlinQueryProviderBase : IGremlinQueryProvider
        {
            private readonly IGremlinQueryProvider _baseGremlinQueryProvider;

            protected GremlinQueryProviderBase(IGremlinQueryProvider baseGremlinQueryProvider)
            {
                this._baseGremlinQueryProvider = baseGremlinQueryProvider;
            }

            public virtual IGremlinQuery CreateQuery()
            {
                return this._baseGremlinQueryProvider
                    .CreateQuery()
                    .ReplaceProvider(this);
            }

            // ReSharper disable once MemberHidesStaticFromOuterClass
            public virtual IAsyncEnumerable<T> Execute<T>(IGremlinQuery<T> query)
            {
                return this._baseGremlinQueryProvider.Execute(query);
            }

            public virtual IGraphModel Model => this._baseGremlinQueryProvider.Model;
        }

        private sealed class JsonSupportGremlinQueryProvider : GremlinQueryProviderBase
        {
            private sealed class MemberInfoMappingsContractResolver : DefaultContractResolver
            {
                private readonly IImmutableDictionary<MemberInfo, string> _mappings;

                public MemberInfoMappingsContractResolver(IImmutableDictionary<MemberInfo, string> mappings)
                {
                    this._mappings = mappings;
                }

                protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
                {
                    var property = base.CreateProperty(member, memberSerialization);

                    this._mappings
                        .TryGetValue(member)
                        .IfSome(
                            mapping =>
                            {
                                property.PropertyName = mapping;
                            });

                    return property;
                }
            }

            private sealed class TimespanConverter : JsonConverter
            {
                public override bool CanConvert(Type objectType)
                {
                    return objectType == typeof(TimeSpan);
                }

                public override bool CanRead => true;
                public override bool CanWrite => true;

                public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
                {
                    if (objectType != typeof(TimeSpan))
                        throw new ArgumentException();

                    var spanString = reader.Value as string;
                    if (spanString == null)
                        return null;
                    return XmlConvert.ToTimeSpan(spanString);
                }

                public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
                {
                    var duration = (TimeSpan)value;
                    writer.WriteValue(XmlConvert.ToString(duration));
                }
            }

            public JsonSupportGremlinQueryProvider(IGremlinQueryProvider baseProvider) : base(baseProvider)
            {
            }

            // ReSharper disable once MemberHidesStaticFromOuterClass
            public override IAsyncEnumerable<T> Execute<T>(IGremlinQuery<T> query)
            {
                var serializer = new JsonSerializer
                {
                    Converters = { new TimespanConverter() },
                    ContractResolver = new MemberInfoMappingsContractResolver(query.MemberInfoMappings),
                    TypeNameHandling = TypeNameHandling.Auto,
                };

                return base
                    .Execute(query.Cast<string>())
                    .Select(json => json.StartsWith("{") || json.StartsWith("[")
                        ? serializer.Deserialize<T>(new JTokenReader(this.TransformToken(JToken.Parse(json))))
                        : JToken.Parse($"'{json}'").ToObject<T>());
            }

            private JToken TransformToken(JToken token)
            {
                if (token is JObject rootObject)
                    return this.TransformObject(rootObject);

                if (token is JArray array)
                {
                    for (var i = 0; i < array.Count; i++)
                    {
                        array[i] = this.TransformToken(array[i]);
                    }

                    return array;
                }

                return token;
            }

            private JObject TransformObject(JObject obj)
            {
                foreach (var propertyKvp in obj)
                {
                    if (propertyKvp.Value is JObject propertyKvpValue)
                        obj[propertyKvp.Key] = this.TransformObject(propertyKvpValue);
                }

                var type = obj["type"]?.ToString().ToLower();

                if (type == "vertex" || type == "edge")
                {
                    obj.Remove("id");
                    var label = obj["label"]?.ToString();
                    if (label != null)
                    {
                        var maybeSuitableType = type == "vertex"
                            ? this.Model.TryGetVertexTypeOfLabel(label)
                            : this.Model.TryGetEdgeTypeOfLabel(label);

                        maybeSuitableType
                            .IfSome(suitableType =>
                            {
                                obj.AddFirst(new JProperty("$type", suitableType.AssemblyQualifiedName));
                                obj.Remove("type");
                            });
                    }

                    if (obj["properties"] is JObject properties)
                    {
                        foreach (var propertyKvp in properties)
                        {
                            var valueObject = propertyKvp.Value;

                            if (propertyKvp.Value is JArray valueObjectArray)
                                valueObject = valueObjectArray.First;

                            var realValue = (valueObject as JObject)?["value"];
                            if (realValue != null)
                                obj[propertyKvp.Key] = realValue;

                            obj.Remove("properties");
                        }
                    }
                }

                return obj;
            }
        }

        private sealed class ModelGremlinQueryProvider : GremlinQueryProviderBase
        {
            public ModelGremlinQueryProvider(IGremlinQueryProvider baseProvider, IGraphModel newModel) : base(baseProvider)
            {
                this.Model = newModel;
            }

            public override IGraphModel Model { get; }
        }

        private sealed class SelectCreateQueryQueryProvider : GremlinQueryProviderBase
        {
            private readonly Func<IGremlinQuery, IGremlinQuery> _projection;

            public SelectCreateQueryQueryProvider(IGremlinQueryProvider baseGremlinQueryProvider, Func<IGremlinQuery, IGremlinQuery> projection) : base(baseGremlinQueryProvider)
            {
                this._projection = projection;
            }

            public override IGremlinQuery CreateQuery()
            {
                return this._projection(base
                    .CreateQuery()
                    .ReplaceProvider(this));
            }
        }

        public static IAsyncEnumerable<T> Execute<T>(this IGremlinQuery<T> query, IGremlinQueryProvider provider)
        {
            return provider.Execute(query);
        }

        public static IGremlinQueryProvider WithJsonSupport(this IGremlinQueryProvider provider)
        {
            return new JsonSupportGremlinQueryProvider(provider);
        }

        public static IGremlinQueryProvider WithModel(this IGremlinQueryProvider provider, IGraphModel model)
        {
            return new ModelGremlinQueryProvider(provider, model);
        }

        public static IGremlinQueryProvider WithSubgraphStrategy(this IGremlinQueryProvider provider, Func<IGremlinQuery<Unit>, IGremlinQuery> vertexCriterion, Func<IGremlinQuery<Unit>, IGremlinQuery> edgeCriterion)
        {
            return provider
                .SelectCreateQuery(query =>
                {
                    var castedQuery = query
                        .Cast<Unit>();

                    var vertexCriterionTraversal = vertexCriterion(castedQuery.ToAnonymous());
                    var edgeCriterionTraversal = edgeCriterion(castedQuery.ToAnonymous());

                    var strategy = GremlinQuery
                        .Create("SubgraphStrategy")
                        .AddStep<Unit>("build");

                    if (vertexCriterionTraversal.Steps.Count > 0)
                        strategy = strategy.AddStep<Unit>("vertices", vertexCriterionTraversal);

                    if (edgeCriterionTraversal.Steps.Count > 0)
                        strategy = strategy.AddStep<Unit>("edges", edgeCriterionTraversal);

                    return query
                        .AddStep<Unit>("withStrategies", strategy.AddStep<Unit>("create"));
                });
        }

        private static IGremlinQueryProvider SelectCreateQuery(this IGremlinQueryProvider provider, Func<IGremlinQuery, IGremlinQuery> projection)
        {
            return new SelectCreateQueryQueryProvider(provider, projection);
        }
    }
}