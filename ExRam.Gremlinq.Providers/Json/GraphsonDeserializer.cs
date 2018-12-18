using System;
using System.Reflection;
using System.Xml;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.GraphElements;
using LanguageExt;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using NullGuard;
using System.Linq;

namespace ExRam.Gremlinq.Providers
{
    public sealed class GraphsonDeserializer : JsonSerializer
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

            public override object ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
            {
                return XmlConvert.ToTimeSpan(serializer.Deserialize<string>(reader));
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }

            public override bool CanRead => true;
            public override bool CanWrite => true;
        }

        private sealed class DateTimeOffsetConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(DateTimeOffset);
            }

            public override object ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
            {
                return System.DateTimeOffset.FromUnixTimeMilliseconds(serializer.Deserialize<long>(reader));
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }
        }

        private sealed class DateTimeConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(DateTime);
            }

            public override object ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
            {
                return new DateTime(System.DateTimeOffset.FromUnixTimeMilliseconds(serializer.Deserialize<long>(reader)).Ticks, DateTimeKind.Utc);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }
        }

        private sealed class ScalarConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return !objectType.IsArray && (objectType.IsValueType || objectType == typeof(string)) && !objectType.IsGenericType;
            }

            public override object ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
            {
                var token = JToken.Load(reader);

                if (token is JArray array)
                {
                    if (array.Count != 1)
                    {
                        if (objectType == typeof(Unit))
                            return Unit.Default;

                        throw new JsonReaderException($"Cannot convert array of length {array.Count} to scalar value.");
                    }

                    token = array[0];
                }

                return ToObject(token, objectType);
            }

            private static object ToObject(JToken token, Type objectType)
            {
                while (true)
                {
                    if (token is JObject jObject && jObject.ContainsKey("value"))
                    {
                        token = jObject["value"];
                        continue;
                    }

                    if (token is JValue value && value.Value is DateTime dateTime && objectType == typeof(long))
                        return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();

                    return token.ToObject(objectType);
                }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }

            public override bool CanRead => true;
            public override bool CanWrite => false;
        }

        private sealed class MetaPropertyConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return typeof(IMeta).IsAssignableFrom(objectType);
            }

            public override object ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
            {
                var token = JToken.Load(reader);

                if (token is JArray array)
                {
                    if (array.Count != 1)
                    {
                        if (objectType == typeof(Unit))
                            return Unit.Default;

                        throw new JsonReaderException($"Cannot convert array of length {array.Count} to scalar value.");
                    }

                    token = array[0];
                }

                return (object)Populate((dynamic)Activator.CreateInstance(objectType, true), token);
            }

            private static Meta<TMeta> Populate<TMeta>(Meta<TMeta> meta, JToken jToken)
            {
                if (jToken is JObject jObject)
                {
                    meta.Value = (TMeta)jObject["value"]?.ToObject(typeof(TMeta));

                    if (jObject["properties"] is JObject metaPropertiesObject)
                    {
                        foreach (var metaProperty in metaPropertiesObject.Properties())
                        {
                            meta.Properties.Add(metaProperty.Name, metaProperty.Value.ToObject(typeof(object)));
                        }
                    }
                }
                else
                    meta.Value = (TMeta)jToken.ToObject(typeof(TMeta));

                return meta;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }

            public override bool CanRead => true;
            public override bool CanWrite => false;
        }

        private sealed class ElementConverter : JsonConverter
        {
            private sealed class ModelIndependentJsonSerializer : JsonSerializer
            {
                public ModelIndependentJsonSerializer(JsonConverter[] additionalConverters)
                {
                    foreach (var additionalConverter in additionalConverters)
                    {
                        Converters.Add(additionalConverter);
                    }

                    Converters.Add(TimeSpan);
                    Converters.Add(DateTimeOffset);
                    Converters.Add(DateTime);
                    Converters.Add(Scalar);
                    Converters.Add(MetaProperty);

                    DefaultValueHandling = DefaultValueHandling.Populate;
                    ContractResolver = new GremlinContractResolver();
                }
            }

            private sealed class VertexImpl : IVertex
            {
                [AllowNull] public object Id { get; set; }
                [AllowNull] public string Label { get; set; }
            }

            private sealed class EdgeImpl : IEdge
            {
                [AllowNull] public object Id { get; set; }
                [AllowNull] public string Label { get; set; }
            }

            private readonly JsonSerializer _modelIndependentSerializer;

            private readonly IGraphModel _model;

            public ElementConverter(IGraphModel model, JsonConverter[] additionalConverters)
            {
                _model = model;
                _modelIndependentSerializer = new ModelIndependentJsonSerializer(additionalConverters);
            }

            public override object ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
            {
                var jToken = JToken.Load(reader);

                if (jToken is JObject)
                {
                    var modelType = _model
                        .GetTypes(jToken["label"].ToString())
                        .FirstOrDefault(type => objectType.IsAssignableFrom(type));

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

                return jToken.ToObject(objectType, _modelIndependentSerializer);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }

            public override bool CanConvert(Type objectType)
            {
                return !objectType.IsSealed && (typeof(IElement).IsAssignableFrom(objectType) || _model.GetLabels(objectType, true).Length > 0);
            }

            public override bool CanWrite => false;
        }
        #endregion

        private static readonly JsonConverter TimeSpan = new TimespanConverter();
        private static readonly JsonConverter DateTimeOffset = new DateTimeOffsetConverter();
        private static readonly JsonConverter DateTime = new DateTimeConverter();
        private static readonly JsonConverter Scalar = new ScalarConverter();
        private static readonly JsonConverter MetaProperty = new MetaPropertyConverter();

        public GraphsonDeserializer(IGraphModel model, params JsonConverter[] additionalConverters)
        {
            foreach (var additionalConverter in additionalConverters)
            {
                Converters.Add(additionalConverter);
            }

            Converters.Add(TimeSpan);
            Converters.Add(DateTimeOffset);
            Converters.Add(DateTime);
            Converters.Add(Scalar);
            Converters.Add(MetaProperty);

            Converters.Add(new ElementConverter(model, additionalConverters));

            DefaultValueHandling = DefaultValueHandling.Populate;
        }
    }
}
