using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.GremlinServer;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    /// <summary>
    /// The Gremlin Server provider, read through Newtonsoft.Json, without a server: for the classes that only serialize.
    /// Unlike <see cref="GremlinServerContainerFixture"/> it keeps the <c>PartitionKey</c> property, so that the
    /// serialization answers like Core's. A fixture for another server overrides <see cref="ConfigureProvider"/>; one for
    /// another JSON stack overrides <see cref="AddJsonSupport"/>.
    /// </summary>
    public class GremlinServerFixture : GremlinqFixture
    {
        protected virtual IGremlinServerConfigurator ConfigureProvider(IGremlinServerConfigurator configurator) => configurator
            .AtLocalhost();

        protected virtual IGremlinServerConfigurator AddJsonSupport(IGremlinServerConfigurator configurator) => configurator
            .UseNewtonsoftJson();

        protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
            .UseGremlinServer<Vertex, Edge>(configurator => AddJsonSupport(ConfigureProvider(configurator)));
    }
}
