using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using ExRam.Gremlinq.Core.GraphElements;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
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

                public void SetValue(object target, object? value)
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

                public void SetValue(object target, object? value)
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

                if (_model.Metadata.TryGetValue(member, out var name))
                    property.PropertyName = name.Name;

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

        private sealed class TimespanConverter : IJTokenConverter
        {
            public bool TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse, out object? value)
            {
                if (objectType == typeof(TimeSpan))
                {
                    if (recurse.TryConvert(jToken, typeof(string), recurse, out var strValue))
                    {
                        value = XmlConvert.ToTimeSpan((string)strValue);
                        return true;
                    }
                }

                value = null;
                return false;
            }
        }

        private sealed class DateTimeOffsetConverter : IJTokenConverter
        {
            public bool TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse, out object? value)
            {
                if (objectType == typeof(DateTimeOffset))
                {
                    if (jToken is JValue jValue)
                    {
                        switch (jValue.Value)
                        {
                            case DateTime dateTime:
                            {
                                value = new DateTimeOffset(dateTime);
                                return true;
                            }
                            case DateTimeOffset dateTimeOffset:
                            {
                                value = dateTimeOffset;
                                return true;
                            }
                            default:
                            {
                                if (jValue.Type == JTokenType.Integer)
                                {
                                    value = DateTimeOffset.FromUnixTimeMilliseconds(jValue.ToObject<long>());
                                    return true;
                                }

                                break;
                            }
                        }
                    }
                }

                value = null;
                return default;
            }
        }

        private sealed class DateTimeConverter : IJTokenConverter
        {
            public bool TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse, out object? value)
            {
                if (objectType == typeof(DateTime))
                {
                    if (jToken is JValue jValue)
                    {
                        switch (jValue.Value)
                        {
                            case DateTime dateTime:
                            {
                                value = dateTime;
                                return true;
                            }
                            case DateTimeOffset dateTimeOffset:
                            {
                                value = dateTimeOffset.UtcDateTime;
                                return true;
                            }
                        }

                        if (jValue.Type == JTokenType.Integer)
                        {
                            value = new DateTime(DateTimeOffset.FromUnixTimeMilliseconds(jValue.ToObject<long>()).Ticks, DateTimeKind.Utc);
                            return true;
                        }
                    }
                }

                value = null;
                return false;
            }
        }

        private sealed class FlatteningConverter : IJTokenConverter
        {
            public bool TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse, out object? value)
            {
                if (!objectType.IsArray)
                {
                    if (!objectType.IsInstanceOfType(jToken))
                    {
                        var itemType = default(Type);

                        if (objectType.IsGenericType)
                        {
                            var genericTypeDefinition = objectType.GetGenericTypeDefinition();
                            if (genericTypeDefinition == typeof(Nullable<>))
                                itemType = objectType.GetGenericArguments()[0];
                        }

                        if (jToken is JArray array)
                        {
                            if (array.Count != 1)
                            {
                                if (array.Count == 0 && (objectType.IsClass || itemType != null))
                                {
                                    value = default(object);
                                    return true;
                                }

                                throw new JsonReaderException($"Cannot convert array\r\n\r\n{array}\r\n\r\nto scalar value of type {objectType}.");
                            }

                            return recurse.TryConvert(array[0], itemType ?? objectType, recurse, out value);
                        }

                        if (jToken is JValue jValue && jValue.Value == null && itemType != null)
                        {
                            value = null;
                            return true;
                        }
                    }
                }

                value = null;
                return false;
            }
        }

        private sealed class ElementConverter : IJTokenConverter
        {
            private sealed class VertexImpl : IVertex
            {
                public object? Id { get; set; }
            }

            private sealed class EdgeImpl : IEdge
            {
                public object? Id { get; set; }
            }

            private readonly IDictionary<string, Type[]> _types;

            public ElementConverter(IGraphModel model)
            {
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

            public bool TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse, out object? value)
            {
                if (jToken is JObject)
                {
                    var label = jToken["label"]?.ToString();

                    var modelType = label != null && _types.TryGetValue(label, out var types)
                        ? types.FirstOrDefault(type => objectType.IsAssignableFrom(type))
                        : default;

                    if (modelType == null)
                    {
                        if (objectType == typeof(IVertex))
                            modelType = typeof(VertexImpl);
                        else if (objectType == typeof(IEdge))
                            modelType = typeof(EdgeImpl);
                    }

                    if (modelType != null && modelType != objectType)
                        return recurse.TryConvert(jToken, modelType, recurse, out value);
                }

                value = null;
                return false;
            }
        }

        private sealed class NativeTypeConverter : IJTokenConverter
        {
            private readonly HashSet<Type> _nativeTypes;

            public NativeTypeConverter(HashSet<Type> nativeTypes)
            {
                _nativeTypes = nativeTypes;
            }

            public bool TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse, out object? value)
            {
                if (_nativeTypes.Contains(objectType) || (objectType.IsEnum && _nativeTypes.Contains(objectType.GetEnumUnderlyingType())))
                {
                    if (jToken is JObject jObject && jObject.ContainsKey("value"))
                    {
                        return recurse.TryConvert(jObject["value"], objectType, recurse, out value);
                    }
                }

                value = null;
                return false;
            }
        }
        
        private sealed class ThisConverter : IJTokenConverter
        {
            private readonly JsonSerializer _populatingSerializer;
            private readonly JsonSerializer _ignoringSerializer;

            public ThisConverter(JsonSerializer populatingSerializer, JsonSerializer ignoringSerializer)
            {
                _populatingSerializer = populatingSerializer;
                _ignoringSerializer = ignoringSerializer;
            }

            public bool TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse, out object? value)
            {
                var ret = jToken.ToObject(objectType, _populatingSerializer);

                if (!(ret is JToken) && jToken is JObject element)
                {
                    if (element.ContainsKey("id") && element.TryGetValue("label", out var label) && label.Type == JTokenType.String && element["properties"] is { } propertiesToken)
                    {
                        _ignoringSerializer.Populate(new JTokenReader(propertiesToken), ret);
                    }
                }

                value = ret;
                return true;
            }
        }

        private sealed class JTokenConverterConverter : JsonConverter
        {
            private readonly IJTokenConverter _converter;

            [ThreadStatic]
            // ReSharper disable once StaticMemberInGenericType
            private static bool _isBlocked;

            public JTokenConverterConverter(IJTokenConverter converter)
            {
                _converter = converter;
            }

            public override bool CanConvert(Type objectType)
            {
                if (_isBlocked)
                {
                    _isBlocked = false;

                    return false;
                }

                return true;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }

            public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
            {
                var token = JToken.Load(reader);

                try
                {
                    _isBlocked = true;

                    return _converter.TryConvert(token, objectType, _converter, out var value)
                        ? value
                        : default;
                }
                finally
                {
                    _isBlocked = false;
                }
            }
        }

        private sealed class ArrayConverter : IJTokenConverter
        {
            public bool TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse, out object? value)
            {
                if (objectType.IsArray && jToken is JArray jArray)
                {
                    var elementType = objectType.GetElementType();
                    var array = new ArrayList(jArray.Count);

                    foreach (var jArrayItem in jArray)
                    {
                        var bulk = 1;
                        var effectiveArrayItem = jArrayItem;

                        if (jArrayItem is JObject traverserObject && traverserObject.TryGetValue("@type", out var nestedType) && "g:Traverser".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase) && traverserObject.TryGetValue("@value", out var valueToken) && valueToken is JObject nestedTraverserObject)
                        {
                            if (nestedTraverserObject.TryGetValue("bulk", out var bulkToken))
                            {
                                if (recurse.TryConvert(bulkToken, typeof(int), recurse, out var bulkObject) && bulkObject != null)
                                    bulk = (int)bulkObject;
                            }

                            if (nestedTraverserObject.TryGetValue("value", out var traverserValue))
                            {
                                effectiveArrayItem = traverserValue;
                            }
                        }

                        if (recurse.TryConvert(effectiveArrayItem, elementType, recurse, out var item))
                        {
                            for (var i = 0; i < bulk; i++)
                            {
                                array.Add(item);
                            }
                        }
                    }

                    value = array.ToArray(elementType);
                    return true;
                }

                value = null;
                return false;
            }
        }

        private sealed class MapConverter : IJTokenConverter
        {
            public bool TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse, out object? value)
            {
                if (jToken is JObject jObject && jObject.TryGetValue("@type", out var nestedType) && "g:Map".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase))
                {
                    if (jObject.TryGetValue("@value", out var valueToken) && valueToken is JArray mapArray)
                    {
                        var retObject = new JObject();

                        for (var i = 0; i < mapArray.Count / 2; i++)
                        {
                            retObject.Add(mapArray[i * 2].Value<string>(), mapArray[i * 2 + 1]);
                        }

                        return recurse.TryConvert(retObject, objectType, recurse, out value);
                    }
                }

                value = null;
                return false;
            }
        }

        private sealed class NestedValueConverter : IJTokenConverter
        {
            public bool TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse, out object? value)
            {
                if (jToken is JObject jObject)
                {
                    if (jObject.ContainsKey("@type") && jObject.TryGetValue("@value", out var valueToken))
                        return recurse.TryConvert(valueToken, objectType, recurse, out value);
                }

                value = null;
                return false;
            }
        }
        #endregion

        public GraphsonJsonSerializer(IGremlinQueryEnvironment environment, params IJTokenConverter[] additionalConverters) : this(
            DefaultValueHandling.Populate,
            new IJTokenConverter[]
            {
                new TimespanConverter(),
                new DateTimeOffsetConverter(),
                new DateTimeConverter(),
                new NestedValueConverter(),
                new NativeTypeConverter(new HashSet<Type>(environment.Model.NativeTypes)),
                new FlatteningConverter(),
                new ElementConverter(environment.Model),
                new MapConverter(),
                new ArrayConverter()
            },
            additionalConverters,
            new GremlinContractResolver(environment.Model.PropertiesModel))
        {

        }

        private GraphsonJsonSerializer(
            DefaultValueHandling defaultValueHandling,
            IEnumerable<IJTokenConverter> mandatoryConverters,
            IEnumerable<IJTokenConverter> additionalConverters,
            IContractResolver contractResolver)
        {
            var converter = JTokenConverter
                .Null
                .Combine(new ThisConverter(
                    this,
                    defaultValueHandling == DefaultValueHandling.Populate
                        ? new GraphsonJsonSerializer(
                            DefaultValueHandling.Ignore,
                            mandatoryConverters,
                            additionalConverters,
                            contractResolver)
                        : this));

            foreach (var additionalConverter in mandatoryConverters)
            {
                converter = converter
                    .Combine(additionalConverter);
            }

            foreach (var additionalConverter in additionalConverters)
            {
                converter = converter
                    .Combine(additionalConverter);
            }

            ContractResolver = contractResolver;
            DefaultValueHandling = defaultValueHandling;
            Converters.Add(new JTokenConverterConverter(converter));
        }
    }
}
