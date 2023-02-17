using System.Reflection;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ExRam.Gremlinq.Core
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
                    private readonly IGraphElementPropertyModel _model;

                    public GremlinContractResolver(IGraphElementPropertyModel model)
                    {
                        _model = model;
                    }

                    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
                    {
                        var property = base.CreateProperty(member, memberSerialization);

                        if (_model.MemberMetadata.TryGetValue(member, out var key) && key.Key.RawKey is string name)
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
                            }
                        }

                        property.Readable = false;

                        return property;
                    }
                }

                internal sealed class JTokenConverterConverter : JsonConverter
                {
                    private readonly IGremlinQueryEnvironment _environment;
                    private readonly IGremlinQueryFragmentDeserializer _deserializer;

                    [ThreadStatic]
                    // ReSharper disable once StaticMemberInGenericType
                    internal static bool _canConvert;

                    public JTokenConverterConverter(
                        IGremlinQueryFragmentDeserializer deserializer,
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

                            return _deserializer.TryDeserialize(objectType).From(token, _environment);
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
                    IGremlinQueryFragmentDeserializer fragmentDeserializer)
                {
                    DefaultValueHandling = defaultValueHandling;
                    ContractResolver = new GremlinContractResolver(environment.Model.PropertiesModel);
                    Converters.Add(new JTokenConverterConverter(fragmentDeserializer, environment));
                }
            }

            private readonly ConditionalWeakTable<IGremlinQueryFragmentDeserializer, JsonSerializer> _serializers = new();
            private readonly ConditionalWeakTable<IGremlinQueryFragmentDeserializer, JsonSerializer>.CreateValueCallback _serializerFactory;

            public GremlinQueryEnvironmentCacheImpl(IGremlinQueryEnvironment environment)
            {
                _serializerFactory = closure => new GraphsonJsonSerializer(
                    DefaultValueHandling.Ignore,
                    environment,
                    closure);
            }

            public JsonSerializer GetSerializer(IGremlinQueryFragmentDeserializer fragmentDeserializer)
            {
                GraphsonJsonSerializer.JTokenConverterConverter._canConvert = false;

                return _serializers.GetValue(
                    fragmentDeserializer,
                    _serializerFactory);
            }
        }

        private static readonly ConditionalWeakTable<IGremlinQueryEnvironment, GremlinQueryEnvironmentCacheImpl> Caches = new();

        public static IGremlinQueryEnvironment StoreTimeSpansAsNumbers(this IGremlinQueryEnvironment environment)
        {
            return environment
                .ConfigureSerializer(static serializer => serializer
                    .ConfigureFragmentSerializer(static fragmentSerializer => fragmentSerializer
                        .Override<TimeSpan>(static (t, env, _, recurse) => recurse.Serialize(t.TotalMilliseconds, env))))
                .ConfigureDeserializer(static deserializer => deserializer
                    .Override<JValue, TimeSpan>(static (jValue, env, recurse) => TimeSpan.FromMilliseconds(jValue.Value<double>())));
        }

        internal static JsonSerializer GetJsonSerializer(this IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer deserializer)
        {
            return Caches
                .GetValue(
                    environment,
                    static closure => new GremlinQueryEnvironmentCacheImpl(closure))
                .GetSerializer(deserializer);
        }
    }
}
