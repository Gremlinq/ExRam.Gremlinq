using System;
using System.Collections.Concurrent;
using System.Linq;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public static class GraphElementModelExtensions
    {
        private static readonly ConcurrentDictionary<IGraphElementModel, ConcurrentDictionary<Type, Option<string[]>>> DerivedLabels = new ConcurrentDictionary<IGraphElementModel, ConcurrentDictionary<Type, Option<string[]>>>();

        public static Option<string[]> TryGetFilterLabels(this IGraphElementModel model, Type type)
        {
            return DerivedLabels
                .GetOrAdd(
                    model,
                    _ => new ConcurrentDictionary<Type, Option<string[]>>())
                .GetOrAdd(
                    type,
                    closureType =>
                    {
                        var labels = model.Labels
                            .Where(kvp => !kvp.Key.IsAbstract && closureType.IsAssignableFrom(kvp.Key))
                            .Select(kvp => kvp.Value.LabelOverride.IfNone(kvp.Key.Name))
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
