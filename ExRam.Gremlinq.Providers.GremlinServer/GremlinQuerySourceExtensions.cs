using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using ExRam.Gremlinq.Providers.GremlinServer;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySourceExtensions
    {
        public static IGremlinQueryEnvironment UseGremlinServer(this IGremlinQueryEnvironment environment,
            Action<IWebSocketQuerySourceBuilder>? builderAction = default)
        {
            return environment
                .UseWebSocket(builderAction)
                .ConfigureExecutionPipeline(p => p.ConfigureSerializer(s => s
                    .OverrideFragmentSerializer<IGremlinQueryBase>((query, overridden, recurse) =>
                    {
                        if (query.AsAdmin().Environment.Options.GetValue(GremlinServerGremlinqOptions.WorkaroundTinkerpop2112))
                            query = query.AsAdmin().ConfigureSteps(steps => steps.WorkaroundTINKERPOP_2112().ToImmutableList());

                        return overridden(query);
                    })));
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
