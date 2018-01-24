using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using LanguageExt;
using Newtonsoft.Json.Linq;
using Unit = System.Reactive.Unit;

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

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> UnwrapObject(this IEnumerator<(JsonToken tokenType, object tokenValue)> enumerator, string property, Func<IEnumerator<(JsonToken tokenType, object tokenValue)>, IEnumerator<(JsonToken tokenType, object tokenValue)>> innerTransformation)
        {
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;

                if (current.tokenType == JsonToken.PropertyName)
                {
                    if (property.Equals(current.tokenValue as string, StringComparison.Ordinal))
                    {
                        if (enumerator.MoveNext())
                        {
                            if (enumerator.Current.tokenType == JsonToken.StartObject)
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

                            yield return (JsonToken.PropertyName, property);
                            yield return (enumerator.Current.tokenType, enumerator.Current.tokenValue);

                            continue;
                        }

                        yield break;
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

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> UntilEndObject(this IEnumerator<(JsonToken tokenType, object tokenValue)> source)
        {
            var openObjects = 0;

            while (source.MoveNext())
            {
                switch (source.Current.tokenType)
                {
                    case JsonToken.StartObject:
                        openObjects++;
                        break;
                    case JsonToken.EndObject:
                        if (openObjects == 0)
                            yield break;

                        openObjects--;
                        break;
                }

                yield return source.Current;
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
                        switch (source.Current.tokenType)
                        {
                            case JsonToken.StartArray:
                                openArrays++;
                                break;
                            case JsonToken.EndArray:
                                openArrays--;
                                break;
                        }

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
        private abstract class TypedGremlinQueryProviderBase : ITypedGremlinQueryProvider
        {
            private readonly ITypedGremlinQueryProvider _baseTypedGremlinQueryProvider;

            protected TypedGremlinQueryProviderBase(ITypedGremlinQueryProvider baseTypedGremlinQueryProvider)
            {
                this._baseTypedGremlinQueryProvider = baseTypedGremlinQueryProvider;
            }

            // ReSharper disable once MemberHidesStaticFromOuterClass
            public virtual IAsyncEnumerable<T> Execute<T>(IGremlinQuery<T> query)
            {
                return this._baseTypedGremlinQueryProvider.Execute(query);
            }

            public IGraphModel Model => this._baseTypedGremlinQueryProvider.Model;
            
            public IGremlinQuery<Unit> TraversalSource => this._baseTypedGremlinQueryProvider.TraversalSource;
        }

        private sealed class JsonSupportTypedGremlinQueryProvider : ITypedGremlinQueryProvider
        {
            private readonly IModelGremlinQueryProvider _baseProvider;

            private sealed class JsonGremlinDeserializer : IGremlinDeserializer
            {
                private readonly IGremlinQuery _query;

                private sealed class StepLabelMappingsContractResolver : DefaultContractResolver
                {
                    private readonly IImmutableDictionary<string, StepLabel> _mappings;

                    public StepLabelMappingsContractResolver(IImmutableDictionary<string, StepLabel> mappings)
                    {
                        this._mappings = mappings;
                    }

                    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
                    {
                        var property = base.CreateProperty(member, memberSerialization);

                        this._mappings
                            .TryGetValue(member.Name)
                            .IfSome(
                                mapping =>
                                {
                                    property.PropertyName = mapping.Label;
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

                        if (!(reader.Value is string spanString))
                            return null;

                        return XmlConvert.ToTimeSpan(spanString);
                    }

                    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
                    {
                        var duration = (TimeSpan)value;
                        writer.WriteValue(XmlConvert.ToString(duration));
                    }
                }

                private sealed class AssumeUtcDateTimeOffsetConverter : JsonConverter
                {
                    public override bool CanConvert(Type objectType)
                    {
                        return objectType == typeof(DateTimeOffset);
                    }

                    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
                    {
                        return reader.Value != null
                            ? DateTimeOffset.Parse(reader.Value.ToString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)
                            : (object)null;
                    }

                    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
                    {
                        writer.WriteValue(((DateTimeOffset)value).ToString(serializer.DateFormatString));
                    }
                }

                private sealed class AssumeUtcDateTimeConverter : JsonConverter
                {
                    public override bool CanConvert(Type objectType)
                    {
                        return objectType == typeof(DateTimeOffset);
                    }

                    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
                    {
                        return reader.Value != null
                            ? DateTime.Parse(reader.Value.ToString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)
                            : (object)null;
                    }

                    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
                    {
                        writer.WriteValue(((DateTime)value).ToString(serializer.DateFormatString));
                    }
                }

                public JsonGremlinDeserializer(IGremlinQuery query)
                {
                    this._query = query;
                }
                
                public IAsyncEnumerable<T> Deserialize<T>(string rawData, IGraphModel model)
                {
                    var serializer = new JsonSerializer
                    {
                        Converters = { new TimespanConverter(), new AssumeUtcDateTimeOffsetConverter(), new AssumeUtcDateTimeConverter() },
                        ContractResolver = new StepLabelMappingsContractResolver(this._query.StepLabelMappings),
                        TypeNameHandling = TypeNameHandling.Auto,
                        MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
                    };
                    
                    return AsyncEnumerable.Return(rawData.StartsWith("{") || rawData.StartsWith("[")
                        ? serializer.Deserialize<T>(new JsonTextReader(new StringReader(rawData))
                            .ToTokenEnumerable()
                            .Apply(e => e
                                .UnwrapObject(
                                    "id",
                                    idSection => idSection
                                        .UntilEndObject())
                                .UnwrapObject(
                                    "properties",
                                    propertiesSection => propertiesSection
                                        .UntilEndObject()
                                        .SelectPropertyNode(prop => prop
                                            .TakeOne(y => y
                                                .ExtractProperty("value"))))
                                .SelectToken(tuple => tuple.tokenType == JsonToken.PropertyName && "label".Equals(tuple.tokenValue)
                                    ? (JsonToken.PropertyName, "$type")
                                    : tuple)
                                .SelectPropertyNode("$type", typeNode => typeNode
                                    .SelectToken(tuple =>
                                    {
                                        if (tuple.tokenType == JsonToken.String)
                                        {
                                            return model
                                                .TryGetElementTypeOfLabel(tuple.tokenValue as string)
                                                .Map(suitableType => (JsonToken.String, (object)suitableType.AssemblyQualifiedName))
                                                .IfNone(tuple);
                                        }

                                        return tuple;
                                    })))
                            .ToJsonReader())
                        : JToken.Parse($"'{rawData}'").ToObject<T>());
                }
            }

            public JsonSupportTypedGremlinQueryProvider(IModelGremlinQueryProvider baseProvider)
            {
                this._baseProvider = baseProvider;
            }

            // ReSharper disable once MemberHidesStaticFromOuterClass
            public IAsyncEnumerable<T> Execute<T>(IGremlinQuery<T> query)
            {
                return this._baseProvider
                    .Execute(query)
                    .SelectMany(rawData => new JsonGremlinDeserializer(query)
                        .Deserialize<T>(rawData, this._baseProvider.Model));
            }

            public IGraphModel Model => this._baseProvider.Model;
            
            public IGremlinQuery<Unit> TraversalSource => this._baseProvider.TraversalSource;
        }

        private sealed class ModelGremlinQueryProvider : IModelGremlinQueryProvider
        {
            private readonly INativeGremlinQueryProvider _baseProvider;

            public ModelGremlinQueryProvider(INativeGremlinQueryProvider baseProvider, IGraphModel newModel)
            {
                this.Model = newModel;
                this._baseProvider = baseProvider;
            }

            public IAsyncEnumerable<string> Execute(IGremlinQuery query)
            {
                var serialized = query
                    .Resolve(this.Model)
                    .Serialize();

                return this._baseProvider
                    .Execute(serialized.queryString, serialized.parameters);
            }

            public IGraphModel Model { get; }
            public IGremlinQuery<Unit> TraversalSource => this._baseProvider.TraversalSource;
        }

        private sealed class SubgraphStrategyQueryProvider : TypedGremlinQueryProviderBase
        {
            private readonly Func<IGremlinQuery<Unit>, IGremlinQuery> _edgeCriterion;
            private readonly Func<IGremlinQuery<Unit>, IGremlinQuery> _vertexCriterion;

            public SubgraphStrategyQueryProvider(ITypedGremlinQueryProvider baseTypedGremlinQueryProvider, Func<IGremlinQuery<Unit>, IGremlinQuery> vertexCriterion, Func<IGremlinQuery<Unit>, IGremlinQuery> edgeCriterion) : base(baseTypedGremlinQueryProvider)
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

        private sealed class RewriteStepsQueryProvider<TStep> : TypedGremlinQueryProviderBase where TStep : NonTerminalGremlinStep
        {
            private readonly Func<TStep, Option<IEnumerable<GremlinStep>>> _replacementStepFactory;

            public RewriteStepsQueryProvider(ITypedGremlinQueryProvider baseTypedGremlinQueryProvider, Func<TStep, Option<IEnumerable<GremlinStep>>> replacementStepFactory) : base(baseTypedGremlinQueryProvider)
            {
                this._replacementStepFactory = replacementStepFactory;
            }

            public override IAsyncEnumerable<T> Execute<T>(IGremlinQuery<T> query)
            {
                return base.Execute(RewriteSteps(query).Cast<T>());
            }
                    
            private IGremlinQuery RewriteSteps(IGremlinQuery query)
            {
                return query
                    .RewriteSteps(step =>
                    {
                        if (step is TStep replacedStep)
                            return this._replacementStepFactory(replacedStep);

                        return Option<IEnumerable<GremlinStep>>.None;
                    });
            }
        }

        public static IAsyncEnumerable<T> Execute<T>(this IGremlinQuery<T> query, ITypedGremlinQueryProvider provider)
        {
            return provider.Execute(query);
        }

        public static ITypedGremlinQueryProvider WithJsonSupport(this IModelGremlinQueryProvider provider)
        {
            return new JsonSupportTypedGremlinQueryProvider(provider);
        }

        public static IModelGremlinQueryProvider WithModel(this INativeGremlinQueryProvider provider, IGraphModel model)
        {
            return new ModelGremlinQueryProvider(provider, model);
        }

        public static ITypedGremlinQueryProvider WithSubgraphStrategy(this ITypedGremlinQueryProvider provider, Func<IGremlinQuery<Unit>, IGremlinQuery> vertexCriterion, Func<IGremlinQuery<Unit>, IGremlinQuery> edgeCriterion)
        {
            return new SubgraphStrategyQueryProvider(provider, vertexCriterion, edgeCriterion);
        }
        
        public static ITypedGremlinQueryProvider ReplaceElementProperty<TSource, TProperty>(this ITypedGremlinQueryProvider provider, Func<TSource, bool> overrideCriterion, Expression<Func<TSource, TProperty>> memberExpression, TProperty value)
        {
            return provider
                .DecorateElementProperty(overrideCriterion, step => new ReplaceElementPropertyStep<TSource, TProperty>(step, memberExpression, value));
        }

        public static ITypedGremlinQueryProvider SetDefautElementProperty<TSource, TProperty>(this ITypedGremlinQueryProvider provider, Func<TSource, bool> overrideCriterion, Expression<Func<TSource, TProperty>> memberExpression, TProperty value)
        {
            return provider
                .DecorateElementProperty(overrideCriterion, step => new SetDefaultElementPropertyStep<TSource, TProperty>(step, memberExpression, value));
        }

        public static ITypedGremlinQueryProvider DecorateElementProperty<TSource, TProperty>(this ITypedGremlinQueryProvider provider, Func<TSource, bool> overrideCriterion, Func<AddElementPropertiesStep, DecorateAddElementPropertiesStep<TSource, TProperty>> replacementStepFactory)
        {
            return provider
                .RewriteSteps<AddElementPropertiesStep>(step =>
                {
                    if (step.Element is TSource source)
                    {
                        if (overrideCriterion(source))
                            return new[] { replacementStepFactory(step) };
                    }

                    return Option<IEnumerable<GremlinStep>>.None;
                });
        }

        public static ITypedGremlinQueryProvider RewriteSteps<TStep>(this ITypedGremlinQueryProvider provider, Func<TStep, Option<IEnumerable<GremlinStep>>> replacementStepFactory) where TStep : NonTerminalGremlinStep
        {
            return new RewriteStepsQueryProvider<TStep>(provider, replacementStepFactory);
        }
    }
}