using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core.GraphElements;
using Gremlin.Net.Process.Traversal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ExRam.Gremlinq.Core
{
    internal static class GremlinQueryEnvironmentCache
    {
        private sealed class GremlinQueryEnvironmentCacheImpl : IGremlinQueryEnvironmentCache
        {
            private sealed class GraphsonJsonSerializer : JsonSerializer
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

                        if (_model.MemberMetadata.TryGetValue(member, out var key) && key.Key.RawKey is string name)
                            property.PropertyName = name;

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
                    private readonly IGremlinQueryFragmentDeserializer _deserializer;

                    [ThreadStatic]
                    // ReSharper disable once StaticMemberInGenericType
                    private static bool _canConvert;

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

                    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
                    {
                        throw new NotSupportedException($"Cannot write to {nameof(JTokenConverterConverter)}.");
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
                    IGremlinQueryFragmentDeserializer fragmentDeserializer)
                {
                    DefaultValueHandling = defaultValueHandling;
                    ContractResolver = new GremlinContractResolver(environment.Model.PropertiesModel);
                    Converters.Add(new JTokenConverterConverter(fragmentDeserializer, environment));
                }
            }

            private sealed class KeyLookup
            {
                private static readonly Dictionary<string, T> DefaultTs = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase)
                {
                    { "id", T.Id },
                    { "label", T.Label }
                };

                private readonly HashSet<T> _configuredTs;
                private readonly IGraphElementPropertyModel _model;
                private readonly ConcurrentDictionary<MemberInfo, Key> _members = new ConcurrentDictionary<MemberInfo, Key>();

                public KeyLookup(IGraphElementPropertyModel model)
                {
                    _model = model;
                    _configuredTs = new HashSet<T>(model.MemberMetadata
                        .Where(kvp => kvp.Value.Key.RawKey is T)
                        .ToDictionary(kvp => (T)kvp.Value.Key.RawKey, kvp => kvp.Key)
                        .Keys);
                }

                public Key GetKey(MemberInfo member)
                {
                    return _members.GetOrAdd(
                        member,
                        (closureMember, closureModel) =>
                        {
                            var name = closureMember.Name;

                            if (closureModel.MemberMetadata.TryGetValue(closureMember, out var metadata))
                            {
                                if (metadata.Key.RawKey is T t)
                                    return t;

                                name = (string)metadata.Key.RawKey;
                            }

                            return DefaultTs.TryGetValue(name, out var defaultT) && !_configuredTs.Contains(defaultT)
                                ? (Key)defaultT
                                : name;
                        },
                        _model);
                }
            }

            private readonly KeyLookup _keyLookup;
            private readonly IGremlinQueryEnvironment _environment;
            private readonly ConditionalWeakTable<IGremlinQueryFragmentDeserializer, JsonSerializer> _populatingSerializers = new ConditionalWeakTable<IGremlinQueryFragmentDeserializer, JsonSerializer>();
            private readonly ConditionalWeakTable<IGremlinQueryFragmentDeserializer, JsonSerializer> _ignoringSerializers = new ConditionalWeakTable<IGremlinQueryFragmentDeserializer, JsonSerializer>();
            private readonly ConcurrentDictionary<Type, (PropertyInfo propertyInfo, Key key, SerializationBehaviour serializationBehaviour)[]> _typeProperties = new ConcurrentDictionary<Type, (PropertyInfo, Key, SerializationBehaviour)[]>();

            private readonly ConditionalWeakTable<IGremlinQueryFragmentDeserializer, JsonSerializer>.CreateValueCallback _populatingSerializerFactory;
            private readonly ConditionalWeakTable<IGremlinQueryFragmentDeserializer, JsonSerializer>.CreateValueCallback _ignorigingSerializerFactory;

            public GremlinQueryEnvironmentCacheImpl(IGremlinQueryEnvironment environment)
            {
                _environment = environment;

                _populatingSerializerFactory = closure => new GraphsonJsonSerializer(
                    DefaultValueHandling.Populate,
                    _environment,
                    closure);

                _ignorigingSerializerFactory = closure => new GraphsonJsonSerializer(
                    DefaultValueHandling.Ignore,
                    _environment,
                    closure);

                ModelTypes = environment.Model
                    .VerticesModel
                    .Metadata
                    .Concat(environment.Model.EdgesModel.Metadata)
                    .GroupBy(x => x.Value.Label)
                    .ToDictionary(
                        group => group.Key,
                        group => group
                            .Select(x => x.Key)
                            .ToArray(),
                        StringComparer.OrdinalIgnoreCase);

                FastNativeTypes = environment.Model.NativeTypes
                    .ToDictionary(x => x, x => default(object?));

                _keyLookup = new KeyLookup(_environment.Model.PropertiesModel);
            }

            public JsonSerializer GetPopulatingJsonSerializer(IGremlinQueryFragmentDeserializer fragmentDeserializer)
            {
                return _populatingSerializers.GetValue(
                    fragmentDeserializer,
                    _populatingSerializerFactory);
            }

            public JsonSerializer GetIgnoringJsonSerializer(IGremlinQueryFragmentDeserializer fragmentDeserializer)
            {
                return _ignoringSerializers.GetValue(
                    fragmentDeserializer,
                    _ignorigingSerializerFactory);
            }

            public (PropertyInfo propertyInfo, Key key, SerializationBehaviour serializationBehaviour)[] GetSerializationData(Type type)
            {
                return _typeProperties
                    .GetOrAdd(
                        type,
                        (closureType, closureEnvironment) => closureType
                            .GetTypeHierarchy()
                            .SelectMany(typeInHierarchy => typeInHierarchy
                                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                            .Where(p => p.GetMethod.GetBaseDefinition() == p.GetMethod)
                            .Select(p => (
                                property: p,
                                key: closureEnvironment.GetCache().GetKey(p),
                                serializationBehaviour: closureEnvironment.Model.PropertiesModel.MemberMetadata
                                    .GetValueOrDefault(p, new MemberMetadata(p.Name)).SerializationBehaviour))
                            .OrderBy(x => x.property.Name)
                            .ToArray(),
                        _environment);
            }

            public IReadOnlyDictionary<Type, object?> FastNativeTypes { get; }

            public Key GetKey(MemberInfo member) => _keyLookup.GetKey(member);

            public IReadOnlyDictionary<string, Type[]> ModelTypes { get; }
        }

        private static readonly ConditionalWeakTable<IGremlinQueryEnvironment, IGremlinQueryEnvironmentCache> Caches = new ConditionalWeakTable<IGremlinQueryEnvironment, IGremlinQueryEnvironmentCache>();

        public static IGremlinQueryEnvironmentCache GetCache(this IGremlinQueryEnvironment environment)
        {
            return Caches.GetValue(
                environment,
                closure => new GremlinQueryEnvironmentCacheImpl(closure));
        }
    }
}
