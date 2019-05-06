using System;
using System.Collections.Concurrent;
using System.Linq;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public static class GraphElementModelExtensions
    {
        private static readonly ConcurrentDictionary<IGraphElementModel, ConcurrentDictionary<Type, Option<string[]>>> _derivedLabels = new ConcurrentDictionary<IGraphElementModel, ConcurrentDictionary<Type, Option<string[]>>>();

        public static Option<string[]> TryGetFilterLabels(this IGraphElementModel model, Type type)
        {
            return _derivedLabels
                .GetOrAdd(
                    model,
                    _ => new ConcurrentDictionary<Type, Option<string[]>>())
                .GetOrAdd(
                    type,
                    closureType =>
                    {
                        var labels = model.Labels
                            .Where(kvp => !kvp.Key.IsAbstract && closureType.IsAssignableFrom(kvp.Key))
                            .Select(kvp => kvp.Value)
                            .OrderBy(x => x)
                            .ToArray();

                        return labels.Length == 0
                            ? default(Option<string[]>)
                            : labels.Length == model.Labels.Count
                                ? Array.Empty<string>()
                                : labels;
                    });
        }
    }
}
