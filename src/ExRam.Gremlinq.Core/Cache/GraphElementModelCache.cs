using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core
{
    internal static class GraphElementModelCache
    {
        private sealed class GraphElementModelCacheImpl : IGraphElementModelCache
        {
            private readonly IGraphElementModel _model;
            private readonly ConcurrentDictionary<Type, string> _dict = new();
            private readonly ConcurrentDictionary<Type, ImmutableArray<string>> _derivedLabels = new();

            public GraphElementModelCacheImpl(IGraphElementModel model)
            {
                _model = model;
            }

            public string GetLabel(Type type)
            {
                return _dict
                    .GetOrAdd(
                        type,
                        (closureType, closureModel) => closureType
                            .GetTypeHierarchy()
                            .Where(type => !type.IsAbstract)
                            .Select(type =>
                                closureModel.Metadata.TryGetValue(type, out var metadata)
                                    ? metadata.Label
                                    : null)
                            .FirstOrDefault() ?? closureType.Name,
                        _model);
            }

            public ImmutableArray<string> GetDerivedLabels(Type type)
            {
                return _derivedLabels
                    .GetOrAdd(
                        type,
                        (closureType, closureModel) => closureModel.Metadata
                            .Where(kvp => !kvp.Key.IsAbstract && closureType.IsAssignableFrom(kvp.Key))
                            .Select(kvp => kvp.Value.Label)
                            .OrderBy(x => x)
                            .ToImmutableArray(),
                        _model);
            }
        }

        private static readonly ConditionalWeakTable<IGraphElementModel, IGraphElementModelCache> Caches = new();

        public static IGraphElementModelCache GetCache(this IGraphElementModel model)
        {
            return Caches.GetValue(
                model,
                closure => new GraphElementModelCacheImpl(closure));
        }
    }
}
