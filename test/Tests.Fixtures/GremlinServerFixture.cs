using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Providers.GremlinServer;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class GremlinServerFixture : GremlinqFixture
    {
        protected override async Task<IGremlinQuerySource> TransformQuerySource(IGremlinQuerySource g) => g
            .UseGremlinServer<Vertex, Edge>(_ => _
                .AtLocalhost()
                .UseNewtonsoftJson())
            .ConfigureEnvironment(env => env
                .ConfigureModel(model => model
                    .ConfigureElements(v => v
                        .ConfigureElement<Element>(conf => conf
                            .IgnoreAlways(p => p.PartitionKey)))));
    }
}
