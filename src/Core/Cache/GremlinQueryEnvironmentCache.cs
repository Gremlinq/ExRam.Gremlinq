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
            private readonly IGremlinQueryEnvironment _environment;
            private readonly ConcurrentDictionary<MemberInfo, Key> _members = new();
            private readonly ConcurrentDictionary<Type, (PropertyInfo propertyInfo, Key key, SerializationBehaviour serializationBehaviour)[]> _typeProperties = new();

            public GremlinQueryEnvironmentCacheImpl(IGremlinQueryEnvironment environment)
            {
                _environment = environment;

                ModelTypes = new HashSet<Type>(environment.Model
                    .VerticesModel.ElementTypes
                    .Concat(environment.Model.EdgesModel.ElementTypes));
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
                                serializationBehaviour: closureEnvironment.Model.PropertiesModel
                                    .GetMetadata(p).SerializationBehaviour))
                            .OrderBy(static x => x.key)
                            .ToArray(),
                        _environment);
            }

            public Key GetKey(MemberInfo member) => _members.GetOrAdd(
                member,
                static (closureMember, model) =>
                {
                    var name = closureMember.Name;

                    if (model.GetMetadata(closureMember) is { } metadata)
                    {
                        if (metadata.Key.RawKey is T t)
                            return t;

                        name = (string)metadata.Key.RawKey;
                    }

                    var maybeDefaultT = "id".Equals(name, StringComparison.OrdinalIgnoreCase)
                        ? T.Id
                        : "label".Equals(name, StringComparison.OrdinalIgnoreCase)
                            ? T.Label
                            : default;

                    return maybeDefaultT is { } defaultT && !model.Members.Any(memberInfo => model.GetMetadata(memberInfo).Key.RawKey is T t && t.Equals(defaultT))
                        ? defaultT
                        : name;
                },
                _environment.Model.PropertiesModel);

            public HashSet<Type> ModelTypes { get; }
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
