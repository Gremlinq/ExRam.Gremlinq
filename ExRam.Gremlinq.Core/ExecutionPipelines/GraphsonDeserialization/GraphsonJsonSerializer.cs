using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using ExRam.Gremlinq.Core.GraphElements;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core
{
    internal static class JTokenExtensions
    {
        public static JObject? TryUnmap(this JToken jToken)
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

                    return retObject;
                }
            }

            return null;
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

        private sealed class JTokenConverterConverter : JsonConverter
        {
            private readonly IGremlinQueryEnvironment _environment;
            private readonly IQueryFragmentDeserializer _deserializer;

            [ThreadStatic]
            // ReSharper disable once StaticMemberInGenericType
            private static bool _canConvert;

            public JTokenConverterConverter(
                IQueryFragmentDeserializer deserializer,
                IGremlinQueryEnvironment environment)
            {
                _deserializer = deserializer;
                _environment = environment;
            }

            public override bool CanConvert(Type objectType)
            {
                if (!_canConvert)
                {
                    _canConvert = true;

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
                    _canConvert = false;

                    return _deserializer.TryDeserialize(token, objectType, _environment);
                }
                finally
                {
                    _canConvert = true;
                }
            }
        }
        #endregion

        public GraphsonJsonSerializer(
            DefaultValueHandling defaultValueHandling,
            IGremlinQueryEnvironment environment,
            IQueryFragmentDeserializer fragmentDeserializer)
        {
            DefaultValueHandling = defaultValueHandling;
            ContractResolver = new GremlinContractResolver(environment.Model.PropertiesModel);
            Converters.Add(new JTokenConverterConverter(fragmentDeserializer, environment));
        }
    }
}
