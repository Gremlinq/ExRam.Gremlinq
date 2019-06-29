using System.Collections.Generic;
using ExRam.Gremlinq.Core;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static bool Contains<TSource>(this IEnumerable<TSource> source, StepLabel<TSource> stepLabel)
        {
            throw new InvalidOperationException($"{nameof(EnumerableExtensions)}.{nameof(Contains)} is not intended to be executed. It's use is only valid within expressions.");
        }

        public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> source, StepLabel<TSource[]> stepLabel)
        {
            throw new InvalidOperationException($"{nameof(EnumerableExtensions)}.{nameof(Intersect)} is not intended to be executed. It's use is only valid within expressions.");
        }

        internal static IEnumerable<Step> HandleAnonymousQueries(this IEnumerable<Step> steps)
        {
            using (var e = steps.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    yield return e.Current;

                    if (!e.MoveNext())
                    {
                        yield return IdentityStep.Instance;
                        yield break;
                    }

                    do
                    {
                        yield return e.Current;
                    } while (e.MoveNext());
                }
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
