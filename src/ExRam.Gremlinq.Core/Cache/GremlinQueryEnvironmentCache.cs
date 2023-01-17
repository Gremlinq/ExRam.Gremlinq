using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core.Models;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal static class GremlinQueryEnvironmentCache
    {
        private sealed class GremlinQueryEnvironmentCacheImpl : IGremlinQueryEnvironmentCache
        {
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
                        .Where(static kvp => kvp.Value.Key.RawKey is T)
                        .ToDictionary(static kvp => (T)kvp.Value.Key.RawKey, static kvp => kvp.Key)
                        .Keys);
                }

                public Key GetKey(MemberInfo member)
                {
                    return _members.GetOrAdd(
                        member,
                        static (closureMember, @this) =>
                        {
                            var name = closureMember.Name;

                            if (@this._model.MemberMetadata.TryGetValue(closureMember, out var metadata))
                            {
                                if (metadata.Key.RawKey is T t)
                                    return t;

                                name = (string)metadata.Key.RawKey;
                            }

                            return DefaultTs.TryGetValue(name, out var defaultT) && !@this._configuredTs.Contains(defaultT)
                                ? defaultT
                                : name;
                        },
                        this);
                }
            }

            private readonly KeyLookup _keyLookup;
            private readonly IGremlinQueryEnvironment _environment;
            private readonly ConcurrentDictionary<Type, (PropertyInfo propertyInfo, Key key, SerializationBehaviour serializationBehaviour)[]> _typeProperties = new();

            public GremlinQueryEnvironmentCacheImpl(IGremlinQueryEnvironment environment)
            {
                _environment = environment;

                ModelTypes = new HashSet<Type>(environment.Model
                    .VerticesModel.Metadata.Keys
                    .Concat(environment.Model.EdgesModel.Metadata.Keys));

                ModelTypesForLabels = environment.Model
                    .VerticesModel
                    .Metadata
                    .Concat(environment.Model.EdgesModel.Metadata)
                    .GroupBy(static x => x.Value.Label)
                    .ToDictionary(
                        static group => group.Key,
                        static group => group
                            .Select(static x => x.Key)
                            .ToArray(),
                        StringComparer.OrdinalIgnoreCase);

                FastNativeTypes = environment.Model.NativeTypes
                    .ToDictionary(static x => x, static _ => default(object?));

                _keyLookup = new KeyLookup(_environment.Model.PropertiesModel);
            }

            public (PropertyInfo propertyInfo, Key key, SerializationBehaviour serializationBehaviour)[] GetSerializationData(Type type)
            {
                return _typeProperties
                    .GetOrAdd(
                        type,
                        static (closureType, closureEnvironment) => closureType
                            .GetTypeHierarchy()
                            .SelectMany(static typeInHierarchy => typeInHierarchy
                                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                            .Where(static p => p.GetMethod?.GetBaseDefinition() == p.GetMethod)
                            .Select(p => (
                                property: p,
                                key: closureEnvironment.GetCache().GetKey(p),
                                serializationBehaviour: closureEnvironment.Model.PropertiesModel.MemberMetadata
                                    .GetValueOrDefault(p, new MemberMetadata(p.Name)).SerializationBehaviour))
                            .OrderBy(static x => x.key)
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
                static closure => new GremlinQueryEnvironmentCacheImpl(closure));
        }
    }
}
