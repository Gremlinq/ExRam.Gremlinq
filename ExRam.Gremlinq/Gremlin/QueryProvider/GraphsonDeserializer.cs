using System;
using System.Globalization;
using System.Reflection;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Unit = System.Reactive.Unit;

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

        public GraphsonDeserializer()
        {
            DefaultValueHandling = DefaultValueHandling.Populate;
            Converters.Add(new TimespanConverter());
            Converters.Add(new AssumeUtcDateTimeOffsetConverter());
            Converters.Add(new AssumeUtcDateTimeConverter());
            Converters.Add(new ArrayConverter());
            ContractResolver = new GremlinContractResolver();
            TypeNameHandling = TypeNameHandling.Auto;
            MetadataPropertyHandling = MetadataPropertyHandling.Default;
        }
    }
}