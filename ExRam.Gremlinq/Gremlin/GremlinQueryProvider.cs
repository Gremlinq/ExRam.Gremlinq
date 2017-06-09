using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using LanguageExt;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq
{
    public static class JsonExtensions
    {
        private sealed class EnumerableJsonReader : JsonReader
        {
            private readonly IEnumerator<(JsonToken tokenType, object tokenValue)> _enumerator;

            public EnumerableJsonReader(IEnumerator<(JsonToken tokenType, object tokenValue)> enumerator)
            {
                this._enumerator = enumerator;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                    this._enumerator.Dispose();

                base.Dispose(disposing);
            }

            public override bool Read()
            {
                return this._enumerator.MoveNext();
            }

            public override JsonToken TokenType => this._enumerator.Current.tokenType;

            public override object Value => this._enumerator.Current.tokenValue;
        }

        public static IEnumerable<(JsonToken tokenType, object tokenValue)> ToTokenEnumerable(this JsonReader jsonReader)
        {
            while (jsonReader.Read())
                yield return (jsonReader.TokenType, jsonReader.Value);
        }

        public static JsonReader ToJsonReader(this IEnumerable<(JsonToken tokenType, object tokenValue)> enumerable)
        {
            return new EnumerableJsonReader(enumerable.GetEnumerator());
        }

        public static IEnumerable<(JsonToken tokenType, object tokenValue)> Apply(this IEnumerable<(JsonToken tokenType, object tokenValue)> source, Func<IEnumerator<(JsonToken tokenType, object tokenValue)>, IEnumerator<(JsonToken tokenType, object tokenValue)>> transformation)
        {
            using (var e = transformation(source.GetEnumerator()))
            {
                while (e.MoveNext())
                    yield return e.Current;
            }
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> HidePropertyName(this IEnumerator<(JsonToken tokenType, object tokenValue)> enumerator, string property, Func<IEnumerator<(JsonToken tokenType, object tokenValue)>, IEnumerator<(JsonToken tokenType, object tokenValue)>> innerTransformation)
        {
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;

                if (current.tokenType == JsonToken.PropertyName)
                {
                    if (property.Equals(current.tokenValue as string, StringComparison.Ordinal))
                    {
                        using (var inner = innerTransformation(enumerator))
                        {
                            while (inner.MoveNext())
                            {
                                yield return inner.Current;
                            }
                        }

                        continue;
                    }
                }
                
                yield return current;
            }
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> ReadValue(this IEnumerator<(JsonToken tokenType, object tokenValue)> source)
        {
            var openArrays = 0;
            var openObjects = 0;

            while (source.MoveNext())
            {
                var current = source.Current;

                switch (current.tokenType)
                {
                    case JsonToken.StartObject:
                        openObjects++;
                        break;
                    case JsonToken.StartArray:
                        openArrays++;
                        break;
                    case JsonToken.EndObject:
                        openObjects--;
                        break;
                    case JsonToken.EndArray:
                        openArrays--;
                        break;
                }

                yield return current;

                if (openArrays == 0 && openObjects == 0)
                    break;
            }
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> Unwrap(this IEnumerator<(JsonToken tokenType, object tokenValue)> source)
        {
            if (source.MoveNext())
            {
                if (source.Current.tokenType == JsonToken.StartObject)
                {
                    var openObjects = 0;

                    while (source.MoveNext())
                    {
                        if (source.Current.tokenType == JsonToken.StartObject)
                            openObjects++;
                        else if (source.Current.tokenType == JsonToken.EndObject)
                        {
                            if (openObjects == 0)
                                yield break;

                            openObjects--;
                        }

                        yield return source.Current;
                    }
                }
                else
                {
                    yield return source.Current;

                    while (source.MoveNext())
                        yield return source.Current;
                }
            }
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> TakeOne(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, Func<IEnumerator<(JsonToken tokenType, object tokenValue)>, IEnumerator<(JsonToken tokenType, object tokenValue)>> innerTransformation)
        {
            while (source.MoveNext())
            {
                if (source.Current.tokenType == JsonToken.StartArray)
                {
                    var openArrays = 1;

                    using (var e = innerTransformation(source.ReadValue()))
                    {
                        while (e.MoveNext())
                            yield return e.Current;
                    }

                    while (source.MoveNext())
                    {
                        if (source.Current.tokenType == JsonToken.StartArray)
                            openArrays++;
                        else if (source.Current.tokenType == JsonToken.EndArray)
                            openArrays--;

                        if (openArrays == 0)
                            yield break;
                    }
                }
                else
                {
                    yield return source.Current;
                }
            }
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> ExtractProperty(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, string property)
        {
            while (source.MoveNext())
            {
                if (source.Current.tokenType == JsonToken.PropertyName && property.Equals(source.Current.tokenValue as string))
                {
                    using (var e = source.ReadValue())
                    {
                        while (e.MoveNext())
                            yield return e.Current;
                    }

                    break;
                }
            }
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> SelectPropertyNode(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, Func<IEnumerator<(JsonToken tokenType, object tokenValue)>, IEnumerator<(JsonToken tokenType, object tokenValue)>> projection)
        {
            while (source.MoveNext())
            {
                if (source.Current.tokenType == JsonToken.PropertyName)
                {
                    yield return source.Current;

                    using (var e = projection(source.ReadValue()))
                    {
                        while (e.MoveNext())
                            yield return e.Current;
                    }
                }
            }
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> SelectPropertyNode(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, string propertyName, Func<IEnumerator<(JsonToken tokenType, object tokenValue)>, IEnumerator<(JsonToken tokenType, object tokenValue)>> projection)
        {
            while (source.MoveNext())
            {
                if (source.Current.tokenType == JsonToken.PropertyName && propertyName.Equals(source.Current.tokenValue as string))
                {
                    yield return source.Current;

                    using (var e = projection(source.ReadValue()))
                    {
                        while (e.MoveNext())
                            yield return e.Current;
                    }

                    continue;
                }

                yield return source.Current;
            }
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> SelectToken(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, Func<(JsonToken tokenType, object tokenValue), (JsonToken tokenType, object tokenValue)> projection)
        {
            while (source.MoveNext())
            {
                yield return projection(source.Current);
            }
        }
    }

    public static class GremlinQueryProvider
    {
        private abstract class GremlinQueryProviderBase : IGremlinQueryProvider
        {
            private readonly IGremlinQueryProvider _baseGremlinQueryProvider;

            protected GremlinQueryProviderBase(IGremlinQueryProvider baseGremlinQueryProvider)
            {
                this._baseGremlinQueryProvider = baseGremlinQueryProvider;
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
                    MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
                };

                return base
                    .Execute(query.Cast<string>())
                    .Select(json => json.StartsWith("{") || json.StartsWith("[")
                        ? serializer.Deserialize<T>(new JsonTextReader(new StringReader(json))
                            .ToTokenEnumerable()
                            .Apply(e => e
                                .HidePropertyName(
                                    "id",
                                    idSection => idSection
                                        .Unwrap())
                                .HidePropertyName(
                                    "properties",
                                    propertiesSection => propertiesSection
                                        .Unwrap()
                                        .SelectPropertyNode(prop => prop
                                            .TakeOne(y => y
                                                .Unwrap()
                                                .ExtractProperty("value"))))
                                .SelectToken(tuple => tuple.tokenType == JsonToken.PropertyName && "label".Equals(tuple.tokenValue)
                                    ? (JsonToken.PropertyName, "$type")
                                    : tuple)
                                .SelectPropertyNode("$type", typeNode => typeNode
                                    .SelectToken(tuple =>
                                    {
                                        if (tuple.tokenType == JsonToken.String)
                                        {
                                            return this.Model
                                                .TryGetElementTypeOfLabel(tuple.tokenValue as string)
                                                .Map(suitableType => (JsonToken.String, (object)suitableType.AssemblyQualifiedName))
                                                .IfNone(tuple);
                                        }

                                        return tuple;
                                    })))
                            .ToJsonReader())
                        : JToken.Parse($"'{json}'").ToObject<T>());
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

        private sealed class SubgraphStrategyQueryProvider : GremlinQueryProviderBase
        {
            private readonly Func<IGremlinQuery<Unit>, IGremlinQuery> _edgeCriterion;
            private readonly Func<IGremlinQuery<Unit>, IGremlinQuery> _vertexCriterion;

            public SubgraphStrategyQueryProvider(IGremlinQueryProvider baseGremlinQueryProvider, Func<IGremlinQuery<Unit>, IGremlinQuery> vertexCriterion, Func<IGremlinQuery<Unit>, IGremlinQuery> edgeCriterion) : base(baseGremlinQueryProvider)
            {
                this._edgeCriterion = edgeCriterion;
                this._vertexCriterion = vertexCriterion;
            }

            public override IAsyncEnumerable<T> Execute<T>(IGremlinQuery<T> query)
            {
                var castedQuery = query
                    .Cast<Unit>();

                var vertexCriterionTraversal = this._vertexCriterion(castedQuery.ToAnonymous());
                var edgeCriterionTraversal = this._edgeCriterion(castedQuery.ToAnonymous());

                var strategy = GremlinQuery
                    .Create("SubgraphStrategy")
                    .AddStep<Unit>("build");

                if (vertexCriterionTraversal.Steps.Count > 0)
                    strategy = strategy.AddStep<Unit>("vertices", vertexCriterionTraversal);

                if (edgeCriterionTraversal.Steps.Count > 0)
                    strategy = strategy.AddStep<Unit>("edges", edgeCriterionTraversal);

                query = query
                    .InsertStep<T>(0, new TerminalGremlinStep("withStrategies", strategy.AddStep<Unit>("create")));

                return base.Execute(query);
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
            return new SubgraphStrategyQueryProvider(provider, vertexCriterion, edgeCriterion);
        }
    }
}