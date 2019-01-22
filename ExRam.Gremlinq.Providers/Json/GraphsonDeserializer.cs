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
                    var label = jToken["label"]?.ToString();

                    var modelType = label != null
                        ? _model
                            .GetTypes(label)
                            .FirstOrDefault(type => objectType.IsAssignableFrom(type))
                        : null;

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
                return _model.VerticesModel.TryGetFilterLabels(objectType).IsSome || _model.EdgesModel.TryGetFilterLabels(objectType).IsSome;
            }
        }
        #endregion

        public GraphsonDeserializer(IGraphModel model, params JsonConverter[] additionalConverters)
        {
            foreach (var additionalConverter in additionalConverters)
            {
                Converters.Add(additionalConverter);
            }

            Converters.Add(new FlatteningConverter());
            Converters.Add(new TimespanConverter());
            Converters.Add(new DateTimeOffsetConverter());
            Converters.Add(new DateTimeConverter());
            Converters.Add(new ElementConverter(model));

            ContractResolver = new GremlinContractResolver();
            DefaultValueHandling = DefaultValueHandling.Populate;
        }
    }
}
