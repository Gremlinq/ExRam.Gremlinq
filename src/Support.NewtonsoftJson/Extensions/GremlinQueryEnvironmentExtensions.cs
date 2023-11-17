using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Transformation;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    public static class GremlinQueryEnvironmentExtensions
    {
        private sealed class GremlinQueryEnvironmentCacheImpl
        {
            private sealed class GraphsonJsonSerializer : JsonSerializer
            {
                #region Nested
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

                internal sealed class JTokenConverterConverter : JsonConverter
                {
                    private readonly IGremlinQueryEnvironment _environment;
                    private readonly ITransformer _deserializer;

                    [ThreadStatic]
                    // ReSharper disable once StaticMemberInGenericType
                    internal static bool _canConvert;

                    public JTokenConverterConverter(
                        ITransformer deserializer,
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

                    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
                    {
                        throw new NotSupportedException($"Cannot write to {nameof(JTokenConverterConverter)}.");
                    }

                    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
                    {
                        var token = JToken.Load(reader);

                        try
                        {
                            _canConvert = false;

                            return _deserializer.TryTransformTo(objectType).From(token, _environment);
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
                    ITransformer deserializer)
                {
                    DefaultValueHandling = defaultValueHandling;
                    ContractResolver = new GremlinContractResolver(environment.Model);
                    Converters.Add(new JTokenConverterConverter(deserializer, environment));
                }
            }

            private readonly ConditionalWeakTable<ITransformer, JsonSerializer> _serializers = new();
            private readonly ConditionalWeakTable<ITransformer, JsonSerializer>.CreateValueCallback _serializerFactory;

            public GremlinQueryEnvironmentCacheImpl(IGremlinQueryEnvironment environment)
            {
                _serializerFactory = closure => new GraphsonJsonSerializer(
                    DefaultValueHandling.Ignore,
                    environment,
                    closure);
            }

            public JsonSerializer GetSerializer(ITransformer deserializer)
            {
                GraphsonJsonSerializer.JTokenConverterConverter._canConvert = false;

                return _serializers.GetValue(
                    deserializer,
                    _serializerFactory);
            }
        }

        private sealed class NativeTypeSerializerConverterFactory<TNative, TSerialized> : IConverterFactory
        {
            private sealed class NativeTypeSerializerConverter<TTarget> : IConverter<TNative, TTarget>
            {
                private readonly IGremlinQueryEnvironment _environment;
                private readonly Func<TNative, IGremlinQueryEnvironment, ITransformer, ITransformer, TSerialized> _serializer;

                public NativeTypeSerializerConverter(Func<TNative, IGremlinQueryEnvironment, ITransformer, ITransformer, TSerialized> serializer, IGremlinQueryEnvironment environment)
                {
                    _environment = environment;
                    _serializer = serializer;
                }

                public bool TryConvert(TNative source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (_serializer(source, _environment, defer, recurse) is TTarget serialized)
                    {
                        value = serialized;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            private readonly Func<TNative, IGremlinQueryEnvironment, ITransformer, ITransformer, TSerialized> _serializer;

            public NativeTypeSerializerConverterFactory(Func<TNative, IGremlinQueryEnvironment, ITransformer, ITransformer, TSerialized> serializer)
            {
                _serializer = serializer;
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(TSource) == typeof(TNative) && typeof(TTarget).IsAssignableFrom(typeof(TSerialized))
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(NativeTypeSerializerConverter<>).MakeGenericType(typeof(TNative), typeof(TSerialized), typeof(TTarget)), _serializer, environment)
                : default;
        }

        private sealed class NativeTypeDeserializerConverterFactory<TNative> : IConverterFactory
        {
            private sealed class NativeTypeDeserializerConverter<TTarget> : IConverter<JValue, TTarget>
            {
                private readonly IGremlinQueryEnvironment _environment;
                private readonly Func<JValue, IGremlinQueryEnvironment, ITransformer, ITransformer, TNative> _deserializer;

                public NativeTypeDeserializerConverter(Func<JValue, IGremlinQueryEnvironment, ITransformer, ITransformer, TNative> deserializer, IGremlinQueryEnvironment environment)
                {
                    _environment = environment;
                    _deserializer = deserializer;
                }

                public bool TryConvert(JValue source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (_deserializer(source, _environment, defer, recurse) is TTarget deserialized)
                    {
                        value = deserialized;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            private readonly Func<JValue, IGremlinQueryEnvironment, ITransformer, ITransformer, TNative> _deserializer;

            public NativeTypeDeserializerConverterFactory(Func<JValue, IGremlinQueryEnvironment, ITransformer, ITransformer, TNative> deserializer)
            {
                _deserializer = deserializer;
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(TSource) == typeof(JValue) && typeof(TTarget) == typeof(TNative)
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(NativeTypeDeserializerConverter<>).MakeGenericType(typeof(TNative), typeof(TTarget)), _deserializer, environment)
                : default;
        }

        private static readonly ConditionalWeakTable<IGremlinQueryEnvironment, GremlinQueryEnvironmentCacheImpl> Caches = new();

        public static IGremlinQueryEnvironment UseNewtonsoftJson(this IGremlinQueryEnvironment environment)
        {
            return environment
                .ConfigureDeserializer(deserializer => deserializer
                    .UseNewtonsoftJson());
        }

        public static IGremlinQueryEnvironment RegisterNativeType<TNative, TSerialized>(this IGremlinQueryEnvironment environment, Func<TNative, IGremlinQueryEnvironment, ITransformer, ITransformer, TSerialized> serializer, Func<JValue, IGremlinQueryEnvironment, ITransformer, ITransformer, TNative> deserializer)
        {
            return environment
                .ConfigureNativeTypes(_ => _
                    .Add(typeof(TNative)))
                .ConfigureSerializer(_ => _
                    .Add(new NativeTypeSerializerConverterFactory<TNative, TSerialized>(serializer)))
                .ConfigureDeserializer(_ => _
                    .Add(new NativeTypeDeserializerConverterFactory<TNative>(deserializer)));
        }

        internal static JsonSerializer GetJsonSerializer(this IGremlinQueryEnvironment environment, ITransformer deserializer)
        {
            return Caches
                .GetValue(
                    environment,
                    static closure => new GremlinQueryEnvironmentCacheImpl(closure))
                .GetSerializer(deserializer);
        }
    }
}
