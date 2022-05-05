using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core.Models;

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
                        static (closureType, closureModel) => closureType
                            .GetTypeHierarchy()
                            .Where(static type => !type.IsAbstract)
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
                        static (closureType, closureModel) => closureModel.Metadata
                            .Where(kvp => !kvp.Key.IsAbstract && closureType.IsAssignableFrom(kvp.Key))
                            .Select(static kvp => kvp.Value.Label)
                            .OrderBy(static x => x)
                            .ToImmutableArray(),
                        _model);
            }
        }

        private static readonly ConditionalWeakTable<IGraphElementModel, IGraphElementModelCache> Caches = new();

        public static IGraphElementModelCache GetCache(this IGraphElementModel model)
        {
            return Caches.GetValue(
                model,
                static closure => new GraphElementModelCacheImpl(closure));
        }
    }
}
