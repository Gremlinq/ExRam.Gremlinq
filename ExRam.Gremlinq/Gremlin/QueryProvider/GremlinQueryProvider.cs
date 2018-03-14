using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
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
                                using (var inner = innerTransformation(enumerator.UntilEndObject()))
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

                // ReSharper disable once SwitchStatementMissingSomeCases
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
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (source.Current.tokenType)
                {
                    case JsonToken.StartObject:
                    { 
                        openObjects++;
                        break;
                    }
                    case JsonToken.EndObject:
                    { 
                        if (openObjects == 0)
                            yield break;

                        openObjects--;
                        break;
                    }
                }

                yield return source.Current;
            }
        }
        
        public static IEnumerator<(JsonToken tokenType, object tokenValue)> Log(this IEnumerator<(JsonToken tokenType, object tokenValue)> source)
        {
            while (source.MoveNext())
            {
                Debug.WriteLine(source.Current);
                yield return source.Current;
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

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> SelectPropertyValue(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, Func<IEnumerator<(JsonToken tokenType, object tokenValue)>, IEnumerator<(JsonToken tokenType, object tokenValue)>> projection)
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

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> SelectArray(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, Func<IEnumerator<(JsonToken tokenType, object tokenValue)>, IEnumerator<(JsonToken tokenType, object tokenValue)>> projection)
        {
            while (source.MoveNext())
            {
                if (source.Current.tokenType == JsonToken.StartArray || source.Current.tokenType == JsonToken.EndArray)
                {
                    yield return source.Current;

                    if (source.Current.tokenType == JsonToken.EndArray)
                        yield break;
                }
                else
                    source = source.Pushback();

                using (var e = projection(source.ReadValue()))
                {
                    while (e.MoveNext())
                        yield return e.Current;
                }
            }
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> Pushback(this IEnumerator<(JsonToken tokenType, object tokenValue)> source)
        {
            yield return source.Current;

            using (source)
            {
                while (source.MoveNext())
                    yield return source.Current;
            }
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> SelectPropertyValue(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, string propertyName, Func<IEnumerator<(JsonToken tokenType, object tokenValue)>, IEnumerator<(JsonToken tokenType, object tokenValue)>> projection)
        {
            while (source.MoveNext())
            {
                yield return source.Current;

                if (source.Current.tokenType == JsonToken.PropertyName && propertyName.Equals(source.Current.tokenValue as string))
                {
                    using (var e = projection(source.ReadValue()))
                    {
                        while (e.MoveNext())
                            yield return e.Current;
                    }
                }
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

            public virtual IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
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

                private sealed class GremlinContractResolver : DefaultContractResolver
                {
                    private sealed class EmptyListValueProvider : IValueProvider
                    {
                        private readonly object _defaultValue;
                        private readonly IValueProvider _innerProvider;

                        public EmptyListValueProvider(IValueProvider innerProvider, Type elementType)
                        {
                            this._innerProvider = innerProvider;
                            this._defaultValue = Array.CreateInstance(elementType, 0);
                        }

                        public void SetValue(object target, object value)
                        {
                            this._innerProvider.SetValue(target, value ?? this._defaultValue);
                        }

                        public object GetValue(object target)
                        {
                            return this._innerProvider.GetValue(target) ?? this._defaultValue;
                        }
                    }

                    private readonly IImmutableDictionary<StepLabel, string> _mappings;

                    public GremlinContractResolver(IImmutableDictionary<StepLabel, string> mappings)
                    {
                        this._mappings = mappings;
                    }

                    protected override IValueProvider CreateMemberValueProvider(MemberInfo member)
                    {
                        var provider = base.CreateMemberValueProvider(member);

                        if (member is PropertyInfo propertyMember)
                        {
                            var propertyType = propertyMember.PropertyType;

                            if (propertyType.IsArray)
                                return new EmptyListValueProvider(provider, propertyType.GetElementType());
                        }

                        return provider;
                    }

                    //protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
                    //{
                    //    var property = base.CreateProperty(member, memberSerialization);

                    //    this._mappings
                    //        .TryGetValue(member.Name)
                    //        .IfSome(
                    //            mapping =>
                    //            {
                    //                property.PropertyName = mapping.Label;
                    //            });

                    //    return property;
                    //}
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

                        var str = serializer.Deserialize<string>(reader);

                        return double.TryParse(str, out var number)
                            ? TimeSpan.FromSeconds(number)
                            : XmlConvert.ToTimeSpan(str);
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
                        var stringValue = serializer.Deserialize<string>(reader);

                        return stringValue != null
                            ? DateTimeOffset.Parse(stringValue, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)
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
                        var stringValue = serializer.Deserialize<string>(reader);

                        return stringValue != null
                            ? DateTime.Parse(stringValue, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)
                            : (object)null;
                    }

                    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
                    {
                        writer.WriteValue(((DateTime)value).ToString(serializer.DateFormatString));
                    }
                }

                private sealed class ArrayConverter : JsonConverter
                {
                    public override bool CanConvert(Type objectType)
                    {
                        return objectType.IsArray
                            // ReSharper disable once TailRecursiveCall
                            ? this.CanConvert(objectType.GetElementType())
                            : (objectType.IsValueType || objectType == typeof(string)) && !objectType.IsGenericType;
                    }

                    public override bool CanRead => true;
                    public override bool CanWrite => false;

                    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
                    {
                        var token = JToken.Load(reader);

                        if (token is JArray array && !objectType.IsArray)
                        {
                            if (array.Count != 1)
                            {
                                if (objectType == typeof(LanguageExt.Unit))
                                    return LanguageExt.Unit.Default;

                                if (objectType == typeof(Unit))
                                    return Unit.Default;

                                throw new JsonReaderException($"Cannot convert array of length {array.Count} to scalar value.");
                            }

                            return array[0].ToObject(objectType);
                        }

                        return token.ToObject(objectType);
                    }

                    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
                    {
                        throw new NotSupportedException();
                    }
                }

                private static readonly JsonConverter Converter1 = new TimespanConverter();
                private static readonly JsonConverter Converter2 = new AssumeUtcDateTimeOffsetConverter();
                private static readonly JsonConverter Converter3 = new AssumeUtcDateTimeConverter();
                private static readonly JsonConverter Converter4 = new ArrayConverter();

                public JsonGremlinDeserializer(IGremlinQuery query)
                {
                    this._query = query;
                }

                public IAsyncEnumerable<TElement> Deserialize<TElement>(string rawData, IGraphModel model)
                {
                    var serializer = new JsonSerializer
                    {
                        DefaultValueHandling = DefaultValueHandling.Populate,
                        Converters = { Converter1, Converter2, Converter3, Converter4 },
                        ContractResolver = new GremlinContractResolver(this._query.StepLabelMappings),
                        TypeNameHandling = TypeNameHandling.Auto,
                        MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
                    };

                    return serializer
                        .Deserialize<TElement[]>(new JsonTextReader(new StringReader(rawData))
                            .ToTokenEnumerable()
                            .Apply(e => e
                                .UnwrapObject(
                                    "id",
                                    idSection => idSection)
                                .UnwrapObject(
                                    "properties",
                                    propertiesSection => propertiesSection
                                        .SelectPropertyValue(array => array
                                            .SelectArray(arrayItem => arrayItem
                                                .ExtractProperty("value"))))
                                .SelectToken(tuple => tuple.tokenType == JsonToken.PropertyName && "label".Equals(tuple.tokenValue)
                                    ? (JsonToken.PropertyName, "$type")
                                    : tuple)
                                .SelectPropertyValue("$type", typeNode => typeNode
                                    .SelectToken(tuple => tuple.tokenType == JsonToken.String
                                        ? model
                                            .TryGetElementTypeOfLabel(tuple.tokenValue as string)
                                            .Map(suitableType => (JsonToken.String, (object)suitableType.AssemblyQualifiedName))
                                            .IfNone(tuple)
                                        : tuple)))
                            .ToJsonReader())
                        .ToAsyncEnumerable();
                }
            }

            public JsonSupportTypedGremlinQueryProvider(IModelGremlinQueryProvider baseProvider)
            {
                this._baseProvider = baseProvider;
            }

            public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
            {
                return this._baseProvider
                    .Execute(query)
                    .SelectMany(rawData => new JsonGremlinDeserializer(query)
                        .Deserialize<TElement>(rawData, this._baseProvider.Model));
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
            private readonly Option<TerminalGremlinStep> _maybeSubgraphStrategyStep; 

            public SubgraphStrategyQueryProvider(ITypedGremlinQueryProvider baseTypedGremlinQueryProvider, Func<IGremlinQuery<Unit>, IGremlinQuery> vertexCriterion, Func<IGremlinQuery<Unit>, IGremlinQuery> edgeCriterion) : base(baseTypedGremlinQueryProvider)
            {
                var vertexCriterionTraversal = vertexCriterion(GremlinQuery.Anonymous);
                var edgeCriterionTraversal = edgeCriterion(GremlinQuery.Anonymous);

                if (vertexCriterionTraversal.Steps.Count > 0 || edgeCriterionTraversal.Steps.Count > 0)
                {
                    var strategy = GremlinQuery
                        .Create("SubgraphStrategy")
                        .AddStep<Unit>("build");

                    if (vertexCriterionTraversal.Steps.Count > 0)
                        strategy = strategy.AddStep<Unit>("vertices", vertexCriterionTraversal);

                    if (edgeCriterionTraversal.Steps.Count > 0)
                        strategy = strategy.AddStep<Unit>("edges", edgeCriterionTraversal);

                    this._maybeSubgraphStrategyStep = new TerminalGremlinStep("withStrategies", strategy.AddStep<Unit>("create"));
                }
            }

            public override IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
            {
                return base.Execute(this._maybeSubgraphStrategyStep
                    .Fold(query, (_, subgraphStrategyStep) => _.InsertStep<TElement>(0, subgraphStrategyStep)));
            }
        }

        private sealed class RewriteStepsQueryProvider<TStep> : TypedGremlinQueryProviderBase where TStep : NonTerminalGremlinStep
        {
            private readonly Func<TStep, Option<IEnumerable<GremlinStep>>> _replacementStepFactory;

            public RewriteStepsQueryProvider(ITypedGremlinQueryProvider baseTypedGremlinQueryProvider, Func<TStep, Option<IEnumerable<GremlinStep>>> replacementStepFactory) : base(baseTypedGremlinQueryProvider)
            {
                this._replacementStepFactory = replacementStepFactory;
            }

            public override IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
            {
                return base.Execute(RewriteSteps(query).Cast<TElement>());
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

        public static IAsyncEnumerable<TElement> Execute<TElement>(this IGremlinQuery<TElement> query, ITypedGremlinQueryProvider provider)
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