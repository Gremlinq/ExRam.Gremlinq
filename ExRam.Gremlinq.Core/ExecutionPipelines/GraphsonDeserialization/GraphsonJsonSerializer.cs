using System;
using System.Collections;
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
    public interface IJTokenConverter
    {
        OptionUnsafe<object> TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse);
    }

    internal static class JTokenConverter
    {
        private sealed class NullJTokenConverter : IJTokenConverter
        {
            public OptionUnsafe<object> TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse)
            {
                return default;
            }
        }

        private sealed class CombinedJTokenConverter : IJTokenConverter
        {
            private readonly IJTokenConverter _converter1;
            private readonly IJTokenConverter _converter2;

            public CombinedJTokenConverter(IJTokenConverter converter1, IJTokenConverter converter2)
            {
                _converter1 = converter1;
                _converter2 = converter2;
            }

            public OptionUnsafe<object> TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse)
            {
                var ret = _converter2
                    .TryConvert(jToken, objectType, recurse);

                return ret.IsSome
                    ? ret
                    : _converter1.TryConvert(jToken, objectType, recurse);
            }
        }

        public static readonly IJTokenConverter Null = new NullJTokenConverter();

        public static IJTokenConverter Combine(this IJTokenConverter converter1, IJTokenConverter converter2)
        {
            return new CombinedJTokenConverter(converter1, converter2);
        }
    }

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

        // ReSharper disable once UnusedTypeParameter
        private abstract class BlockableConverter<TSelf> : JsonConverter
        {
            [ThreadStatic]
            // ReSharper disable once StaticMemberInGenericType
            private static bool _isBlocked;

            private sealed class BlockDisposable : IDisposable
            {
                public static readonly BlockDisposable Instance = new BlockDisposable();

                public void Dispose()
                {
                    _isBlocked = false;
                }
            }

            public sealed override bool CanConvert(Type objectType)
            {
                if (_isBlocked)
                {
                    _isBlocked = false;

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
                _isBlocked = true;

                return BlockDisposable.Instance;
            }

            protected abstract bool CanConvertImpl(Type objectType);

            public override bool CanRead => true;
            public override bool CanWrite => true;
        }

        private sealed class TimespanConverter : IJTokenConverter
        {
            public OptionUnsafe<object> TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse)
            {
                if (objectType == typeof(TimeSpan))
                {
                    return recurse
                        .TryConvert(jToken, typeof(string), recurse)
                        .Map(x => (object)XmlConvert.ToTimeSpan((string)x));
                }

                return default;
            }
        }

        private sealed class DateTimeOffsetConverter : IJTokenConverter
        {
            public OptionUnsafe<object> TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse)
            {
                if (objectType == typeof(DateTimeOffset))
                {
                    if (jToken is JValue jValue)
                    {
                        switch (jValue.Value)
                        {
                            case DateTime dateTime:
                                return new DateTimeOffset(dateTime);
                            case DateTimeOffset dateTimeOffset:
                                return dateTimeOffset;
                            default:
                            {
                                if (jValue.Type == JTokenType.Integer)
                                    return DateTimeOffset.FromUnixTimeMilliseconds(jValue.ToObject<long>());

                                break;
                            }
                        }
                    }
                }

                return default;
            }
        }

        private sealed class DateTimeConverter : IJTokenConverter
        {
            public OptionUnsafe<object> TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse)
            {
                if (objectType == typeof(DateTime))
                {
                    if (jToken is JValue jValue)
                    {
                        switch (jValue.Value)
                        {
                            case DateTime dateTime:
                                return dateTime;
                            case DateTimeOffset dateTimeOffset:
                                return dateTimeOffset.UtcDateTime;
                        }

                        if (jValue.Type == JTokenType.Integer)
                            return new DateTime(DateTimeOffset.FromUnixTimeMilliseconds(jValue.ToObject<long>()).Ticks, DateTimeKind.Utc);
                    }
                }

                return default;
            }
        }

        private sealed class FlatteningConverter : IJTokenConverter
        {
            public OptionUnsafe<object> TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse)
            {
                if (!objectType.IsArray)
                {
                    if (!objectType.IsInstanceOfType(jToken))
                    {
                        var itemType = default(Type);

                        if (objectType.IsGenericType)
                        {
                            var genericTypeDefinition = objectType.GetGenericTypeDefinition();
                            if (genericTypeDefinition == typeof(Option<>) || genericTypeDefinition == typeof(Nullable<>))
                                itemType = objectType.GetGenericArguments()[0];
                        }

                        if (jToken is JArray array)
                        {
                            if (array.Count != 1)
                            {
                                if (objectType == typeof(Unit))
                                    return Unit.Default;

                                if (array.Count == 0 && (objectType.IsClass || itemType != null))
                                    return default(object);

                                throw new JsonReaderException($"Cannot convert array\r\n\r\n{array}\r\n\r\nto scalar value of type {objectType}.");
                            }

                            return recurse.TryConvert(array[0], itemType ?? objectType, recurse);
                        }

                        if (jToken is JValue value && value.Value == null && itemType != null)
                            return default(object);
                    }
                }

                return default;
            }
        }

        private sealed class ElementConverter : IJTokenConverter
        {
            private sealed class VertexImpl : IVertex
            {
                [AllowNull] public object? Id { get; set; }
            }

            private sealed class EdgeImpl : IEdge
            {
                [AllowNull] public object? Id { get; set; }
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

            public OptionUnsafe<object> TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse)
            {
                if (jToken is JObject)
                {
                    var label = jToken["label"]?.ToString();

                    var modelType = label != null
                        ? _types
                            .TryGetValue(label)
                            .IfNone(Array.Empty<Type>())
                            .FirstOrDefault(type => objectType.IsAssignableFrom(type))
                        : default;

                    if (modelType == null)
                    {
                        if (objectType == typeof(IVertex))
                            modelType = typeof(VertexImpl);
                        else if (objectType == typeof(IEdge))
                            modelType = typeof(EdgeImpl);
                    }

                    if (modelType != null && modelType != objectType)
                        return recurse.TryConvert(jToken, modelType, recurse);
                }

                return default;
            }
        }

        private sealed class NativeTypeConverter : IJTokenConverter
        {
            private readonly System.Collections.Generic.HashSet<Type> _nativeTypes;

            public NativeTypeConverter(System.Collections.Generic.HashSet<Type> nativeTypes)
            {
                _nativeTypes = nativeTypes;
            }

            public OptionUnsafe<object> TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse)
            {
                if (_nativeTypes.Contains(objectType) || (objectType.IsEnum && _nativeTypes.Contains(objectType.GetEnumUnderlyingType())))
                {
                    if (jToken is JObject jObject && jObject.ContainsKey("value"))
                    {
                        return recurse.TryConvert(jObject["value"], objectType, recurse);
                    }
                }

                return default;
            }
        }
        
        private sealed class ThisConverter : IJTokenConverter
        {
            private readonly JsonSerializer _serializer;

            public ThisConverter(JsonSerializer serializer)
            {
                _serializer = serializer;
            }

            public OptionUnsafe<object> TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse)
            {
                return jToken.ToObject(objectType, _serializer);
            }
        }

        private sealed class JTokenConverterConverter : BlockableConverter<JTokenConverterConverter>
        {
            private readonly IJTokenConverter _converter;

            public JTokenConverterConverter(IJTokenConverter converter)
            {
                _converter = converter;
            }

            protected override bool CanConvertImpl(Type objectType)
            {
                return true;
            }

            [return: AllowNull]
            public override object ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
            {
                var token = JToken.Load(reader);

                using (Block())
                {
                    return _converter
                        .TryConvert(token, objectType, _converter)
                        .IfNoneUnsafe(default(object));
                }
            }
        }

        private sealed class ArrayConverter : IJTokenConverter
        {
            public OptionUnsafe<object> TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse)
            {
                if (objectType.IsArray && jToken is JArray jArray)
                {
                    var elementType = objectType.GetElementType();
                    var array = new ArrayList(jArray.Count);

                    foreach (var jArrayItem in jArray)
                    {
                        var bulk = 1;
                        var effectiveArrayItem = jArrayItem;

                        if (jArrayItem is JObject traverserObject && traverserObject.TryGetValue("@type", out var nestedType) && "g:Traverser".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase) && traverserObject.TryGetValue("@value", out var value) && value is JObject nestedTraverserObject)
                        {
                            if (nestedTraverserObject.TryGetValue("bulk", out var bulkToken))
                            {
                                bulk = recurse
                                    .TryConvert(bulkToken, typeof(int), recurse)
                                    .Map(x => (int)x)
                                    .IfNoneUnsafe(1);
                            }

                            if (nestedTraverserObject.TryGetValue("value", out var traverserValue))
                            {
                                effectiveArrayItem = traverserValue;
                            }
                        }

                        var maybeItem = recurse
                            .TryConvert(effectiveArrayItem, elementType, recurse);

                        if (maybeItem.IsSome)
                        {
                            var item = maybeItem.IfNoneUnsafe(default(object));

                            for (var i = 0; i < bulk; i++)
                            {
                                array.Add(item);
                            }
                        }
                    }

                    return array.ToArray(elementType);
                }

                return default;
            }
        }
        #endregion

        public GraphsonJsonSerializer(IGremlinQueryEnvironment environment, params IJTokenConverter[] additionalConverters)
        {
            var converter = JTokenConverter
                .Null
                .Combine(new ThisConverter(this))
                .Combine(new TimespanConverter())
                .Combine(new DateTimeOffsetConverter())
                .Combine(new DateTimeConverter())
                .Combine(new NativeTypeConverter(new System.Collections.Generic.HashSet<Type>(environment.Model.NativeTypes)))
                .Combine(new FlatteningConverter())
                .Combine(new ElementConverter(environment.Model))
                .Combine(new ArrayConverter());

            foreach (var additionalConverter in additionalConverters)
            {
                converter = converter
                    .Combine(additionalConverter);
            }

            Converters.Add(new JTokenConverterConverter(converter));
            DefaultValueHandling = DefaultValueHandling.Populate;
            ContractResolver = new GremlinContractResolver(environment.Model.PropertiesModel);
        }
    }
}
