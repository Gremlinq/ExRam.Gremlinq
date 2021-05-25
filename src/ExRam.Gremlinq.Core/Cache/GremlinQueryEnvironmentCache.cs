using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Models;
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
                private static readonly Dictionary<string, T> DefaultTs = new(StringComparer.OrdinalIgnoreCase)
                {
                    { "id", T.Id },
                    { "label", T.Label }
                };

                private readonly HashSet<T> _configuredTs;
                private readonly IGraphElementPropertyModel _model;
                private readonly ConcurrentDictionary<MemberInfo, Key> _members = new();

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
                        (closureMember, @this) =>
                        {
                            var name = closureMember.Name;

                            if (@this._model.MemberMetadata.TryGetValue(closureMember, out var metadata))
                            {
                                if (metadata.Key.RawKey is T t)
                                    return t;

                                name = (string)metadata.Key.RawKey;
                            }

                            return DefaultTs.TryGetValue(name, out var defaultT) && !@this._configuredTs.Contains(defaultT)
                                ? (Key)defaultT
                                : name;
                        },
                        this);
                }
            }

            private readonly KeyLookup _keyLookup;
            private readonly IGremlinQueryEnvironment _environment;
            private readonly ConditionalWeakTable<IGremlinQueryFragmentDeserializer, JsonSerializer> _ignoringSerializers = new();
            private readonly ConditionalWeakTable<IGremlinQueryFragmentDeserializer, JsonSerializer>.CreateValueCallback _ignoringSerializerFactory;
            private readonly ConcurrentDictionary<Type, (PropertyInfo propertyInfo, Key key, SerializationBehaviour serializationBehaviour)[]> _typeProperties = new();

            public GremlinQueryEnvironmentCacheImpl(IGremlinQueryEnvironment environment)
            {
                _environment = environment;

                _ignoringSerializerFactory = closure => new GraphsonJsonSerializer(
                    DefaultValueHandling.Ignore,
                    _environment,
                    closure);

                ModelTypes = new HashSet<Type>(environment.Model
                    .VerticesModel.Metadata.Keys
                    .Concat(environment.Model.EdgesModel.Metadata.Keys));

                ModelTypesForLabels = environment.Model
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
                    .ToDictionary(x => x, _ => default(object?));

                _keyLookup = new KeyLookup(_environment.Model.PropertiesModel);
            }

            public JsonSerializer GetPopulatingJsonSerializer(IGremlinQueryFragmentDeserializer fragmentDeserializer)
            {
                GraphsonJsonSerializer.JTokenConverterConverter._canConvert = false;

                return _ignoringSerializers.GetValue(
                    fragmentDeserializer,
                    _ignoringSerializerFactory);
            }

            public JsonSerializer GetIgnoringJsonSerializer(IGremlinQueryFragmentDeserializer fragmentDeserializer)
            {
                GraphsonJsonSerializer.JTokenConverterConverter._canConvert = true;

                return _ignoringSerializers.GetValue(
                    fragmentDeserializer,
                    _ignoringSerializerFactory);
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
                            .Where(p => p.GetMethod?.GetBaseDefinition() == p.GetMethod)
                            .Select(p => (
                                property: p,
                                key: closureEnvironment.GetCache().GetKey(p),
                                serializationBehaviour: closureEnvironment.Model.PropertiesModel.MemberMetadata
                                    .GetValueOrDefault(p, new MemberMetadata(p.Name)).SerializationBehaviour))
                            .OrderBy(x => x.key)
                            .ToArray(),
                        _environment);
            }

            public HashSet<Type> ModelTypes { get; }

            public IReadOnlyDictionary<Type, object?> FastNativeTypes { get; }

            public Key GetKey(MemberInfo member) => _keyLookup.GetKey(member);

            public IReadOnlyDictionary<string, Type[]> ModelTypesForLabels { get; }
        }

        private static readonly ConditionalWeakTable<IGremlinQueryEnvironment, IGremlinQueryEnvironmentCache> Caches = new();

        public static IGremlinQueryEnvironmentCache GetCache(this IGremlinQueryEnvironment environment)
        {
            return Caches.GetValue(
                environment,
                closure => new GremlinQueryEnvironmentCacheImpl(closure));
        }
    }
}
