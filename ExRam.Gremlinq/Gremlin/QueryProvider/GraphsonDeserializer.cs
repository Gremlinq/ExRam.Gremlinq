using System;
using System.Collections;
using System.Reflection;
using System.Xml;
using LanguageExt;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace ExRam.Gremlinq
{
    public sealed class GraphsonDeserializer : JsonSerializer
    {
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
                    ? TimeSpan.FromMilliseconds(number)
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
                if (token is JObject jObject && jObject.ContainsKey("value"))
                    return ToObject(jObject["value"], objectType);

                return token.ToObject(objectType);
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
                return objectType.IsConstructedGenericType && objectType.GetGenericTypeDefinition() == typeof(Meta<>);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                JsonSerializer serializer)
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

                if (token is JObject obj)
                {
                    return (object)Populate((dynamic)Activator.CreateInstance(objectType, true), obj);
                }

                throw new NotImplementedException();
            }

            private Meta<T> Populate<T>(Meta<T> meta, JToken jToken)
            {
                if (jToken is JObject jObject)
                {
                    meta.Value = (T)jObject["value"]?.ToObject(typeof(T));

                    if (jObject["properties"] is JObject metaPropertiesObject)
                    {
                        foreach (var metaProperty in metaPropertiesObject.Properties())
                        {
                            meta.Add(metaProperty.Name, metaProperty.Value.ToObject(typeof(object)));
                        }
                    }

                    return meta;
                }

                throw new NotImplementedException();

            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }

            public override bool CanRead => true;
            public override bool CanWrite => false;
        }

        public GraphsonDeserializer()
        {
            DefaultValueHandling = DefaultValueHandling.Populate;
            Converters.Add(new TimespanConverter());
            Converters.Add(new AssumeUtcDateTimeOffsetConverter());
            Converters.Add(new AssumeUtcDateTimeConverter());
            Converters.Add(new ScalarConverter());
            Converters.Add(new MetaPropertyConverter());
            ContractResolver = new GremlinContractResolver();
            TypeNameHandling = TypeNameHandling.Auto;
            MetadataPropertyHandling = MetadataPropertyHandling.Default;
        }
    }
}
