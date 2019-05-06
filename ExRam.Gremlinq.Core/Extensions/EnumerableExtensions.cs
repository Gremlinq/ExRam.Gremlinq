using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using ExRam.Gremlinq.Core;
using LanguageExt;

namespace System.Linq
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

    public static class ImmutableDictionaryExtensions
    {
        public static Option<TValue> TryGetValue<TKey, TValue>(this ImmutableDictionary<TKey, TValue> dict, TKey key)
        {
            return ((IReadOnlyDictionary<TKey, TValue>)dict).TryGetValue(key);
        }
    }

    public static class EnumerableExtensions
    {
        public static bool Contains<TSource>(this IEnumerable<TSource> source, StepLabel<TSource> stepLabel)
        {
            throw new InvalidOperationException($"{nameof(EnumerableExtensions)}.{nameof(Contains)} is not intended to be executed. It's use is only valid within expressions.");
        }

        internal static IEnumerable<Step> HandleAnonymousQueries(this IEnumerable<Step> steps)
        {
            using (var e = steps.GetEnumerator())
            {
                var hasNext = e.MoveNext();

                if (!hasNext || !(e.Current is IdentifierStep))
                    yield return IdentifierStep.__;

                if (!hasNext)
                    yield return IdentityStep.Instance;
                else
                    yield return e.Current;

                while (e.MoveNext())
                    yield return e.Current;
            }
        }

        //https://issues.apache.org/jira/browse/TINKERPOP-2112.
        internal static IEnumerable<Step> WorkaroundTINKERPOP_2112(this IEnumerable<Step> steps)
        {
            var propertySteps = default(List<PropertyStep>);

            using (var e = steps.GetEnumerator())
            {
                while (true)
                {
                    var hasNext = e.MoveNext();

                    if (hasNext && e.Current is PropertyStep propertyStep)
                    {
                        if (propertySteps == null)
                            propertySteps = new List<PropertyStep>();

                        propertySteps.Add(propertyStep);
                    }
                    else
                    {
                        if (propertySteps != null && propertySteps.Count > 0)
                        {
                            propertySteps.Sort((x, y) => -(x.Key is T).CompareTo(y.Key is T));

                            foreach (var replayPropertyStep in propertySteps)
                            {
                                yield return replayPropertyStep;
                            }

                            propertySteps.Clear();
                        }

                        if (hasNext)    
                            yield return e.Current;
                    }

                    if (!hasNext)
                        break;
                }
            }
        }
    }
}
