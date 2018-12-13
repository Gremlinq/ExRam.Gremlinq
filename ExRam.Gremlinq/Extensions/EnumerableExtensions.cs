using System.Collections.Generic;
using ExRam.Gremlinq;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static bool Intersects<T>(this IEnumerable<T> source, IEnumerable<T> other)
        {
            throw new NotImplementedException("This method is to be used within expression and therefore has no implementation.");
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
                        if (propertySteps != null)
                        {
                            propertySteps.Sort((x, y) => -(x.Key is T).CompareTo(y.Key is T));

                            foreach (var replayPropertyStep in propertySteps)
                            {
                                yield return replayPropertyStep;
                            }

                            propertySteps = null;
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
