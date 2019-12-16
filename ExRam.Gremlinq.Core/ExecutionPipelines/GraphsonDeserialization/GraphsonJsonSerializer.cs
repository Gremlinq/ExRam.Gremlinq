using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using ExRam.Gremlinq.Core.GraphElements;
using LanguageExt;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using NullGuard;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    internal sealed class GraphsonJsonSerializer : JsonSerializer
    {
        #region Nested
        private sealed class GremlinContractResolver : DefaultContractResolver
        {
            private sealed class EmptyListValueProvider : IValueProvider
            {
                private readonly object _defaultValue;
                private readonly IValueProvider _innerProvider;

                public EmptyListValueProvider(IValueProvider innerProvider, Type elementType)
                {
                    _innerProvider = innerProvider;
                    _defaultValue = Array.CreateInstance(elementType, 0);
                }

                public void SetValue(object target, [AllowNull] object value)
                {
                    _innerProvider.SetValue(target, value ?? _defaultValue);
                }

                public object GetValue(object target)
                {
                    return _innerProvider.GetValue(target) ?? _defaultValue;
                }
            }

            private sealed class EmptyDictionaryValueProvider : IValueProvider
            {
                private readonly object _defaultValue;
                private readonly IValueProvider _innerProvider;

                public EmptyDictionaryValueProvider(IValueProvider innerProvider)
                {
                    _innerProvider = innerProvider;
                    _defaultValue = new Dictionary<string, object>();
                }

                public void SetValue(object target, [AllowNull] object value)
                {
                    _innerProvider.SetValue(target, value ?? _defaultValue);
                }

                public object GetValue(object target)
                {
                    return _innerProvider.GetValue(target) ?? _defaultValue;
                }
            }

            private readonly IGraphElementPropertyModel _model;

            public GremlinContractResolver(IGraphElementPropertyModel model)
            {
                _model = model;
            }

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);

                _model.Metadata
                    .TryGetValue(member)
                    .Map(x => x.Name)
                    .IfSome(name => property.PropertyName = name);

                return property;
            }

            protected override IValueProvider CreateMemberValueProvider(MemberInfo member)
            {
                var provider = base.CreateMemberValueProvider(member);

                if (member is PropertyInfo propertyMember)
                {
                    var propertyType = propertyMember.PropertyType;

                    if (propertyType == typeof(IDictionary<string, object>) && propertyMember.Name == nameof(VertexProperty<object>.Properties) && typeof(IVertexProperty).IsAssignableFrom(propertyMember.DeclaringType))
                        return new EmptyDictionaryValueProvider(provider);

                    if (propertyType.IsArray)
                        return new EmptyListValueProvider(provider, propertyType.GetElementType());
                }

                return provider;
            }
        }

        private abstract class BlockableConverter : JsonConverter
        {
            [ThreadStatic]
            private static List<JsonConverter>? _blockedConverters;

            private sealed class BlockDisposable : IDisposable
            {
                private readonly JsonConverter _blockedConverter;

                public BlockDisposable(JsonConverter blockedConverter)
                {
                    _blockedConverter = blockedConverter;
                }

                public void Dispose()
                {
                    _blockedConverters?.Remove(_blockedConverter);
                }
            }

            public sealed override bool CanConvert(Type objectType)
            {
                var blocked = (_blockedConverters?.Contains(this)).GetValueOrDefault();

                if (blocked)
                {
                    _blockedConverters?.Remove(this);

                    return false;
                }

                return CanConvertImpl(objectType);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }

            protected IDisposable Block()
            {
                if (_blockedConverters == null)
                    _blockedConverters = new List<JsonConverter>();

                _blockedConverters.Add(this);

                return new BlockDisposable(this);
            }

            protected abstract bool CanConvertImpl(Type objectType);

            public override bool CanRead => true;
            public override bool CanWrite => true;
        }

        private sealed class TimespanConverter : BlockableConverter
        {
            protected override bool CanConvertImpl(Type objectType)
            {
                return objectType == typeof(TimeSpan);
            }

            public override object ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
            {
                return XmlConvert.ToTimeSpan(serializer.Deserialize<string>(reader));
            }
        }

        private sealed class DateTimeOffsetConverter : BlockableConverter
        {
            protected override bool CanConvertImpl(Type objectType)
            {
                return objectType == typeof(DateTimeOffset);
            }

            public override object ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
            {
                var token = JToken.Load(reader);

                if (token is JValue jValue)
                {
                    if (jValue.Value is DateTime dateTime)
                        return new DateTimeOffset(dateTime);

                    if (jValue.Value is DateTimeOffset dateTimeOffset)
                        return dateTimeOffset;

                    if (jValue.Type == JTokenType.Integer)
                        return DateTimeOffset.FromUnixTimeMilliseconds(jValue.ToObject<long>());
                }

                using (Block())
                {
                    return token.ToObject(objectType, serializer);
                }
            }
        }

        private sealed class DateTimeConverter : BlockableConverter
        {
            protected override bool CanConvertImpl(Type objectType)
            {
                return objectType == typeof(DateTime);
            }

            public override object ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
            {
                var token = JToken.Load(reader);

                if (token is JValue jValue)
                {
                    if (jValue.Value is DateTime dateTime)
                        return dateTime;

                    if (jValue.Value is DateTimeOffset dateTimeOffset)
                        return dateTimeOffset.UtcDateTime;

                    if (jValue.Type == JTokenType.Integer)
                        return new DateTime(DateTimeOffset.FromUnixTimeMilliseconds(jValue.ToObject<long>()).Ticks, DateTimeKind.Utc);
                }

                using (Block())
                {
                    return token.ToObject(objectType, serializer);
                }
            }
        }

        private sealed class FlatteningConverter : BlockableConverter
        {
            protected override bool CanConvertImpl(Type objectType)
            {
                return !objectType.IsArray;
            }

            [return: AllowNull]
            public override object? ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
            {
                var token = JToken.Load(reader);

                if (!objectType.IsInstanceOfType(token))
                {
                    if (token is JArray array)
                    {
                        if (array.Count != 1)
                        {
                            if (objectType == typeof(Unit))
                                return Unit.Default;

                            if (array.Count == 0)
                            {
                                if (objectType.IsClass)
                                    return null;

                                if (objectType.IsGenericType)
                                {
                                    var genericTypeDefinition = objectType.GetGenericTypeDefinition();

                                    if (genericTypeDefinition == typeof(Option<>) || genericTypeDefinition == typeof(Nullable<>))
                                        return Activator.CreateInstance(objectType);
                                }
                            }

                            throw new JsonReaderException($"Cannot convert array\r\n\r\n{array}\r\n\r\nto scalar value of type {objectType}.");
                        }

                        token = array[0];
                    }
                }

                if (token is JObject jObject && !typeof(Property).IsAssignableFrom(objectType) && jObject.ContainsKey("value"))
                    token = jObject["value"];

                using (Block())
                {
                    return token.ToObject(objectType, serializer);
                }
            }
        }

        private sealed class ElementConverter : BlockableConverter
        {
            private sealed class VertexImpl : IVertex
            {
                [AllowNull] public object? Id { get; set; }
            }

            private sealed class EdgeImpl : IEdge
            {
                [AllowNull] public object? Id { get; set; }
            }

            private readonly IGraphModel _model;
            private readonly IDictionary<string, Type[]> _types;

            public ElementConverter(IGraphModel model)
            {
                _model = model;
                _types = model
                    .VerticesModel
                    .Metadata
                    .Concat(model.EdgesModel.Metadata)
                    .GroupBy(x => x.Value.Label)
                    .ToDictionary(
                        group => group.Key,
                        group => group
                            .Select(x => x.Key)
                            .ToArray(),
                        StringComparer.OrdinalIgnoreCase);
            }

            public override object ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
            {
                var jToken = JToken.Load(reader);

                if (jToken is JObject)
                {
                    var label = jToken["label"]?.ToString();

                    var modelType = label != null
                        ? _types
                            .TryGetValue(label)
                            .IfNone(Array.Empty<Type>())
                            .FirstOrDefault(type => objectType.IsAssignableFrom(type))
                        : default;

                    if (modelType != null)
                        objectType = modelType;
                    else
                    {
                        if (objectType == typeof(IVertex))
                            objectType = typeof(VertexImpl);
                        else if (objectType == typeof(IEdge))
                            objectType = typeof(EdgeImpl);
                    }
                }

                using (Block())
                { 
                    return jToken.ToObject(objectType, serializer);
                }
            }

            protected override bool CanConvertImpl(Type objectType)
            {
                return _model.VerticesModel.TryGetFilterLabels(objectType, FilterLabelsVerbosity.Maximum).IsSome || _model.EdgesModel.TryGetFilterLabels(objectType, FilterLabelsVerbosity.Maximum).IsSome;
            }
        }
        #endregion

        public GraphsonJsonSerializer(IGremlinQueryEnvironment environment, params JsonConverter[] additionalConverters)
        {
            foreach (var additionalConverter in additionalConverters)
            {
                Converters.Add(additionalConverter);
            }

            Converters.Add(new FlatteningConverter());
            Converters.Add(new TimespanConverter());
            Converters.Add(new DateTimeOffsetConverter());
            Converters.Add(new DateTimeConverter());
            Converters.Add(new ElementConverter(environment.Model));

            ContractResolver = new GremlinContractResolver(environment.Model.PropertiesModel);
            DefaultValueHandling = DefaultValueHandling.Populate;
        }
    }
}
