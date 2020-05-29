using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core
{
    internal static class GraphElementModelExtensions
    {
        private static readonly ConditionalWeakTable<IGraphElementModel, ConcurrentDictionary<Type, string>> Cache = new ConditionalWeakTable<IGraphElementModel, ConcurrentDictionary<Type, string>>();

        public static string GetLabel(this IGraphElementModel elementModel, Type type)
        {
            return Cache
                .GetOrCreateValue(elementModel)
                .GetOrAdd(
                    type,
                    (closureType, closureModel) => closureType
                        .GetTypeHierarchy()
                        .Where(type => !type.IsAbstract)
                        .Select(type => closureModel.Metadata.TryGetValue(type, out var metadata)
                            ? metadata.Label
                            : null)
                        .FirstOrDefault() ?? closureType.Name,
                    elementModel);
        }
    }
}
