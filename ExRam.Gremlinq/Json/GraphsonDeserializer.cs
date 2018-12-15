using System;
using System.Reflection;
using System.Xml;
using ExRam.Gremlinq.GraphElements;
using LanguageExt;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq
{
    internal sealed class GraphsonDeserializer : JsonSerializer
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

                public void SetValue(object target, object value)
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

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (objectType != typeof(TimeSpan))
                    throw new ArgumentException();

                var str = serializer.Deserialize<string>(reader);

                return double.TryParse(str, out var number)
                    ? System.TimeSpan.FromMilliseconds(number)
                    : XmlConvert.ToTimeSpan(str);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(XmlConvert.ToString((TimeSpan)value));
            }

            public override bool CanRead => true;
            public override bool CanWrite => true;
        }

        private sealed class AssumeUtcDateTimeOffsetConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(DateTimeOffset);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return DateTimeOffset.FromUnixTimeMilliseconds(serializer.Deserialize<long>(reader));
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(((DateTimeOffset)value).ToUnixTimeMilliseconds());
            }
        }

        private sealed class AssumeUtcDateTimeConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(DateTime);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var milliseconds = serializer.Deserialize<long>(reader);

                return new DateTime(DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).Ticks, DateTimeKind.Utc);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(((DateTimeOffset)(DateTime)value).ToUnixTimeMilliseconds());
            }
        }

        private sealed class ScalarConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return !objectType.IsArray && (objectType.IsValueType || objectType == typeof(string)) && !objectType.IsGenericType;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
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

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
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
        #endregion

        private sealed class ElementConverter : JsonConverter
        {
            private sealed class ModelIndependentJsonSerializer : JsonSerializer
            {
                public ModelIndependentJsonSerializer()
                {
                    Converters.Add(TimeSpan);
                    Converters.Add(UtcDateTimeOffset);
                    Converters.Add(UtcDateTime);
                    Converters.Add(Scalar);
                    Converters.Add(MetaProperty);

                    DefaultValueHandling = DefaultValueHandling.Populate;
                    ContractResolver = new GremlinContractResolver();
                }
            }

            private static readonly JsonSerializer ModelIndependentSerializer = new ModelIndependentJsonSerializer();

            private readonly IGraphModel _model;

            public ElementConverter(IGraphModel model)
            {
                _model = model;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var jToken = JToken.Load(reader);

                if (jToken is JObject)
                {
                    objectType = _model
                        .TryGetType(jToken["label"].ToString())
                        .Filter(objectType.IsAssignableFrom)
                        .IfNone(() =>
                        {
                            // ReSharper disable AccessToModifiedClosure
                            if (objectType == typeof(Vertex))
                                return typeof(VertexImpl);

                            return objectType == typeof(Edge)
                                ? typeof(EdgeImpl)
                                : objectType;
                            // ReSharper restore AccessToModifiedClosure
                        });
                }

                return jToken.ToObject(objectType, ModelIndependentSerializer);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }

            public override bool CanConvert(Type objectType)
            {
                return !objectType.IsSealed && (typeof(Element).IsAssignableFrom(objectType) || _model.TryGetDerivedLabels(objectType).Length > 0);
            }

            public override bool CanWrite => false;
        }

        private static readonly JsonConverter TimeSpan = new TimespanConverter();
        private static readonly JsonConverter UtcDateTimeOffset = new AssumeUtcDateTimeOffsetConverter();
        private static readonly JsonConverter UtcDateTime = new AssumeUtcDateTimeConverter();
        private static readonly JsonConverter Scalar = new ScalarConverter();
        private static readonly JsonConverter MetaProperty = new MetaPropertyConverter();

        public GraphsonDeserializer(IGraphModel model)
        {
            Converters.Add(TimeSpan);
            Converters.Add(UtcDateTimeOffset);
            Converters.Add(UtcDateTime);
            Converters.Add(Scalar);
            Converters.Add(MetaProperty);
            Converters.Add(new ElementConverter(model));

            DefaultValueHandling = DefaultValueHandling.Populate;
        }
    }
}
