using System;
using System.Collections.Generic;
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
    public static class GremlinQueryProvider
    {
        private sealed class JsonSupportTypedGremlinQueryProvider : ITypedGremlinQueryProvider
        {
            private readonly IModelGremlinQueryProvider<string> _baseProvider;

            private sealed class JsonGremlinDeserializer : IGremlinDeserializer
            {
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

                private static readonly JsonSerializer Deserializer = new JsonSerializer
                {
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    Converters =
                    {
                        new TimespanConverter(),
                        new AssumeUtcDateTimeOffsetConverter(),
                        new AssumeUtcDateTimeConverter(),
                        new ArrayConverter()
                    },
                    ContractResolver = new GremlinContractResolver(),
                    TypeNameHandling = TypeNameHandling.Auto,
                    MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
                };

                public IAsyncEnumerable<TElement> Deserialize<TElement>(string rawData, IGraphModel model)
                {
                    return JsonGremlinDeserializer.Deserializer
                        .Deserialize<TElement[]>(new JsonTextReader(new StringReader(rawData))
                            .ToTokenEnumerable()
                            .Apply(e => e
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

            public JsonSupportTypedGremlinQueryProvider(IModelGremlinQueryProvider<string> baseProvider)
            {
                this._baseProvider = baseProvider;
            }

            public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
            {
                return this._baseProvider
                    .Execute(query)
                    .SelectMany(rawData => new JsonGremlinDeserializer()
                        .Deserialize<TElement>(rawData, this._baseProvider.Model));
            }

            public IGraphModel Model => this._baseProvider.Model;
            
            public IGremlinQuery<Unit> TraversalSource => this._baseProvider.TraversalSource;
        }

        private sealed class ModelGremlinQueryProvider<TNative> : IModelGremlinQueryProvider<TNative>
        {
            private readonly INativeGremlinQueryProvider<TNative> _baseProvider;

            public ModelGremlinQueryProvider(INativeGremlinQueryProvider<TNative> baseProvider, IGraphModel newModel)
            {
                this.Model = newModel;
                this._baseProvider = baseProvider;
            }

            public IAsyncEnumerable<TNative> Execute(IGremlinQuery query)
            {
                var serialized = query
                    .Cast<Unit>()
                    .Resolve(this.Model)
                    .Serialize();

                return this._baseProvider
                    .Execute(serialized.queryString, serialized.parameters);
            }

            public IGraphModel Model { get; }
            public IGremlinQuery<Unit> TraversalSource => this._baseProvider.TraversalSource;
        }

        private sealed class RewriteStepsQueryProvider<TStep, TNative> : IModelGremlinQueryProvider<TNative> where TStep : NonTerminalGremlinStep
        {
            private readonly IModelGremlinQueryProvider<TNative> _baseTypedGremlinQueryProvider;
            private readonly Func<TStep, Option<IEnumerable<GremlinStep>>> _replacementStepFactory;

            public RewriteStepsQueryProvider(IModelGremlinQueryProvider<TNative> baseTypedGremlinQueryProvider, Func<TStep, Option<IEnumerable<GremlinStep>>> replacementStepFactory)
            {
                this._replacementStepFactory = replacementStepFactory;
                this._baseTypedGremlinQueryProvider = baseTypedGremlinQueryProvider;
            }

            public IAsyncEnumerable<TNative> Execute(IGremlinQuery query)
            {
                return this._baseTypedGremlinQueryProvider.Execute(query
                    .Cast<Unit>()
                    .RewriteSteps(step => step is TStep replacedStep
                        ? this._replacementStepFactory(replacedStep)
                        : Option<IEnumerable<GremlinStep>>.None)
                    .Cast<Unit>());
            }

            public IGraphModel Model => this._baseTypedGremlinQueryProvider.Model;

            public IGremlinQuery<Unit> TraversalSource => this._baseTypedGremlinQueryProvider.TraversalSource;
        }

        public static IAsyncEnumerable<TElement> Execute<TElement>(this IGremlinQuery<TElement> query, ITypedGremlinQueryProvider provider)
        {
            return provider.Execute(query);
        }

        public static ITypedGremlinQueryProvider WithJsonSupport(this IModelGremlinQueryProvider<string> provider)
        {
            return new JsonSupportTypedGremlinQueryProvider(provider);
        }

        public static IModelGremlinQueryProvider<TNative> WithModel<TNative>(this INativeGremlinQueryProvider<TNative> provider, IGraphModel model)
        {
            return new ModelGremlinQueryProvider<TNative>(provider, model);
        }
       
        public static IModelGremlinQueryProvider<TNative> ReplaceElementProperty<TSource, TProperty, TNative>(this IModelGremlinQueryProvider<TNative> provider, Func<TSource, bool> overrideCriterion, Expression<Func<TSource, TProperty>> memberExpression, TProperty value)
        {
            return provider
                .DecorateElementProperty(overrideCriterion, step => new ReplaceElementPropertyStep<TSource, TProperty>(step, memberExpression, value));
        }

        public static IModelGremlinQueryProvider<TNative> SetDefautElementProperty<TSource, TProperty, TNative>(this IModelGremlinQueryProvider<TNative> provider, Func<TSource, bool> overrideCriterion, Expression<Func<TSource, TProperty>> memberExpression, TProperty value)
        {
            return provider
                .DecorateElementProperty(overrideCriterion, step => new SetDefaultElementPropertyStep<TSource, TProperty>(step, memberExpression, value));
        }

        public static IModelGremlinQueryProvider<TNative> DecorateElementProperty<TSource, TProperty, TNative>(this IModelGremlinQueryProvider<TNative> provider, Func<TSource, bool> overrideCriterion, Func<AddElementPropertiesStep, DecorateAddElementPropertiesStep<TSource, TProperty>> replacementStepFactory)
        {
            return provider
                .RewriteSteps<AddElementPropertiesStep, TNative>(step =>
                {
                    if (step.Element is TSource source)
                    {
                        if (overrideCriterion(source))
                            return new[] { replacementStepFactory(step) };
                    }

                    return Option<IEnumerable<GremlinStep>>.None;
                });
        }

        public static IModelGremlinQueryProvider<TNative> RewriteSteps<TStep, TNative>(this IModelGremlinQueryProvider<TNative> provider, Func<TStep, Option<IEnumerable<GremlinStep>>> replacementStepFactory) where TStep : NonTerminalGremlinStep
        {
            return new RewriteStepsQueryProvider<TStep, TNative>(provider, replacementStepFactory);
        }
    }
}