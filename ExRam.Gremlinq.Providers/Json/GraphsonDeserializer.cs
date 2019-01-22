using System;
using System.Collections.Generic;
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

        private abstract class BlockableConverter : JsonConverter
        {
            [ThreadStatic]
            private static List<JsonConverter> _blockedConverters;

            private sealed class BlockDisposable : IDisposable
            {
                private readonly JsonConverter _blockedConverter;

                public BlockDisposable(JsonConverter blockedConverter)
                {
                    _blockedConverter = blockedConverter;
                }

                public void Dispose()
                {
                    _blockedConverters.Remove(_blockedConverter);
                }
            }

            public sealed override bool CanConvert(Type objectType)
            {
                return !(_blockedConverters?.Contains(this)).GetValueOrDefault() && CanConvertImpl(objectType);
            }

            protected IDisposable Block()
            {
                if (_blockedConverters == null)
                    _blockedConverters = new List<JsonConverter>();

                _blockedConverters.Add(this);
                return new BlockDisposable(this);
            }

            protected abstract bool CanConvertImpl(Type objectType);
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

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }

            public override bool CanRead => true;
            public override bool CanWrite => true;
        }

        private sealed class DateTimeOffsetConverter : BlockableConverter
        {
            protected override bool CanConvertImpl(Type objectType)
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

        private sealed class DateTimeConverter : BlockableConverter
        {
            protected override bool CanConvertImpl(Type objectType)
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

        private sealed class ScalarConverter : BlockableConverter
        {
            protected override bool CanConvertImpl(Type objectType)
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

                return ToObject(token, objectType, serializer);
            }

            private object ToObject(JToken token, Type objectType, JsonSerializer serializer)
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

                    using (Block())
                    {
                        return token.ToObject(objectType, serializer);
                    }
                }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }

            public override bool CanRead => true;
            public override bool CanWrite => false;
        }

        private sealed class MetaPropertyConverter : BlockableConverter
        {
            protected override bool CanConvertImpl(Type objectType)
            {
                return typeof(Property).IsAssignableFrom(objectType);
            }

            public override object ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
            {
                var jToken = JToken.Load(reader);

                if (jToken is JArray array)
                {
                    if (array.Count != 1)
                    {
                        if (objectType == typeof(Unit))
                            return Unit.Default;

                        throw new JsonReaderException($"Cannot convert array of length {array.Count} to scalar value.");
                    }

                    jToken = array[0];
                }

                using (Block())
                { 
                    return jToken.ToObject(objectType, serializer);
                }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }
            
            public override bool CanRead => true;
            public override bool CanWrite => false;
        }

        private sealed class ElementConverter : BlockableConverter
        {
            private sealed class VertexImpl : IVertex
            {
                [AllowNull] public object Id { get; set; }
            }

            private sealed class EdgeImpl : IEdge
            {
                [AllowNull] public object Id { get; set; }
            }

            private readonly IGraphModel _model;

            public ElementConverter(IGraphModel model)
            {
                _model = model;
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

                using (Block())
                { 
                    return jToken.ToObject(objectType, serializer);
                }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }

            protected override bool CanConvertImpl(Type objectType)
            {
                return !objectType.IsSealed && (typeof(IElement).IsAssignableFrom(objectType) || _model.VerticesModel.TryGetFilterLabels(objectType).IsSome || _model.EdgesModel.TryGetFilterLabels(objectType).IsSome);
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
            Converters.Add(new ElementConverter(model));

            ContractResolver = new GremlinContractResolver();

            DefaultValueHandling = DefaultValueHandling.Populate;
        }
    }
}
