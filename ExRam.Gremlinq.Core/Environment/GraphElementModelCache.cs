using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core
{
    internal static class GraphElementModelCache
    {
        private sealed class GraphElementModelCacheImpl : IGraphElementModelCache
        {
            private readonly IGraphElementModel _model;
            private readonly ConcurrentDictionary<Type, string> _dict = new ConcurrentDictionary<Type, string>();

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
        }

        private static readonly ConditionalWeakTable<IGraphElementModel, IGraphElementModelCache> Caches = new ConditionalWeakTable<IGraphElementModel, IGraphElementModelCache>();

        public static IGraphElementModelCache GetCache(this IGraphElementModel model)
        {
            return Caches.GetValue(
                model,
                closure => new GraphElementModelCacheImpl(closure));
        }
    }
}