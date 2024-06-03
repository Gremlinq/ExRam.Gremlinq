using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    internal static class GremlinqQuerySourceExtensions
    {
        public static IGremlinQuerySource IgnoreCosmosDbSpecificProperties(this IGremlinQuerySource source)
        {
            return source
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureElements(v => v
                            .ConfigureElement<Element>(conf => conf
                                .IgnoreAlways(p => p.PartitionKey)))));
        }
    }
}
