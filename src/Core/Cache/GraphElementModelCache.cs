using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core.Models;

namespace ExRam.Gremlinq.Core
{
    internal static class GraphElementModelCache
    {
        private sealed class GraphElementModelCacheImpl : IGraphElementModelCache
        {
            private readonly IGraphElementModel _model;
            private readonly ConcurrentDictionary<Type, string> _labels = new();
            private readonly ConcurrentDictionary<Type, ImmutableArray<string>> _derivedLabels = new();

            public GraphElementModelCacheImpl(IGraphElementModel model)
            {
                _model = model;
            }

            public string GetLabel(Type type)
            {
                return _labels
                    .GetOrAdd(
                        type,
                        static (closureType, closureModel) => closureType
                            .GetTypeHierarchy()
                            .Where(static type => !type.IsAbstract)
                            .Select(type => closureModel.GetMetadata(type).Label)
                            .FirstOrDefault() ?? closureType.Name,
                        _model);
            }

            public ImmutableArray<string> GetDerivedLabels(Type type)
            {
                return _derivedLabels
                    .GetOrAdd(
                        type,
                        static (closureType, closureModel) => closureModel.ElementTypes
                            .Where(elementType => !elementType.IsAbstract && closureType.IsAssignableFrom(elementType))
                            .Select(elementType => closureModel.GetMetadata(elementType).Label)
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
