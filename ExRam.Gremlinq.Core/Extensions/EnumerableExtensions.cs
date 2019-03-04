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
            var propertySteps = default(List<VertexPropertyStep>);

            using (var e = steps.GetEnumerator())
            {
                while (true)
                {
                    var hasNext = e.MoveNext();

                    if (hasNext && e.Current is VertexPropertyStep propertyStep)
                    {
                        if (propertySteps == null)
                            propertySteps = new List<VertexPropertyStep>();

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
