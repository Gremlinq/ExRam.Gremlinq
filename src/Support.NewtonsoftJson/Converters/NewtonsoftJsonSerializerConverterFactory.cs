using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using Newtonsoft.Json;
using ExRam.Gremlinq.Core.Models;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class NewtonsoftJsonSerializerConverterFactory : IConverterFactory
    {
        private sealed class GraphsonJsonSerializer : JsonSerializer
        {
            private sealed class GremlinContractResolver : DefaultContractResolver
            {
                private sealed class VertexPropertyPropertiesConverter<T> : JsonConverter<T>
                {
                    public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
                    {
                        return serializer.Deserialize<VertexPropertyPropertiesWrapper<T>>(reader) is { HasValue: true, Value: { } value }
                            ? value
                            : default;
                    }

                    public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer) => throw new NotImplementedException();
                }

                private readonly IGraphModel _model;

                public GremlinContractResolver(IGraphModel model)
                {
                    _model = model;
                }

                protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
                {
                    var property = base.CreateProperty(member, memberSerialization);

                    if ((_model.VerticesModel.TryGetMetadata(member) ?? _model.EdgesModel.TryGetMetadata(member)) is { Key.RawKey: string name })
                        property.PropertyName = name;

                    if (member.DeclaringType is { } declaringType)
                    {
                        if (declaringType == typeof(Property))
                        {
                            if (member.Name == nameof(Property.Key))
                                property.Writable = true;
                        }
                        else if (declaringType.IsGenericType && declaringType.GetGenericTypeDefinition() == typeof(VertexProperty<,>))
                        {
                            if (member.Name == nameof(VertexProperty<object>.Id) || member.Name == nameof(VertexProperty<object>.Label))
                                property.Writable = true;
                            else if (member is PropertyInfo { Name: nameof(VertexProperty<object>.Properties) } propertyInfo && !typeof(IDictionary<string, object>).IsAssignableFrom(propertyInfo.PropertyType))
                                property.Converter = (JsonConverter?)Activator.CreateInstance(typeof(VertexPropertyPropertiesConverter<>).MakeGenericType(propertyInfo.PropertyType));
                        }
                    }

                    property.Readable = false;

                    return property;
                }
            }

            private sealed class JTokenConverterConverter : JsonConverter
            {
                private readonly IGremlinQueryEnvironment _environment;

                public JTokenConverterConverter(IGremlinQueryEnvironment environment)
                {
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

                public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
                {
                    throw new NotSupportedException($"Cannot write to {nameof(JTokenConverterConverter)}.");
                }

                public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
                {
                    JToken? token;

                    if (reader is JTokenReader { CurrentToken: { } currentToken })
                    {
                        reader.Skip();
                        token = currentToken;
                    }
                    else
                        token = JToken.Load(reader);

                    try
                    {
                        _canConvert = false;

                        return _environment.Deserializer.TryTransformTo(objectType).From(token, _environment);
                    }
                    finally
                    {
                        _canConvert = true;
                    }
                }
            }

            [ThreadStatic]
            private static bool _canConvert;

            public GraphsonJsonSerializer(IGremlinQueryEnvironment environment)
            {
                DefaultValueHandling = DefaultValueHandling.Ignore;
                ContractResolver = new GremlinContractResolver(environment.Model);
                Converters.Add(new JTokenConverterConverter(environment));
            }

            public T? Deserialize<T>(JToken token)
            {
                _canConvert = false;

                return token.ToObject<T>(this);
            }
        }

        private sealed class NewtonsoftJsonSerializerConverter<TSource, TTarget> : IConverter<TSource, TTarget>
            where TSource : JToken
        {
            private readonly GraphsonJsonSerializer _serializer;

            public NewtonsoftJsonSerializerConverter(IGremlinQueryEnvironment environment)
            {
                _serializer = new GraphsonJsonSerializer(environment);
            }

            public bool TryConvert(TSource source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                if (source is TTarget alreadyRequestedValue)
                {
                    value = alreadyRequestedValue;
                    return true;
                }

                try
                {
                    if (_serializer.Deserialize<TTarget>(source) is { } requestedValue)
                    {
                        value = requestedValue;
                        return true;
                    }
                }
                catch (JsonSerializationException)
                {

                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            return typeof(JToken).IsAssignableFrom(typeof(TSource))
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(NewtonsoftJsonSerializerConverter<,>).MakeGenericType(typeof(TSource), typeof(TTarget)), environment)
                : null;
        }
    }
}
