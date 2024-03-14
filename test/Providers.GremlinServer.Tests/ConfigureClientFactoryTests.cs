using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class ConfigureClientFactoryTests
    {
        [Fact]
        public void ConfigureClient_1()
        {
            GremlinQuerySource.g
                .UseGremlinServer<Vertex, Edge>(builder => builder
                    .ConfigureClientFactory(factory => factory
                        .ConfigureClient((client, env) => client)));
        }

        [Fact]
        public void ConfigureClient_2()
        {
            GremlinQuerySource.g
                .UseGremlinServer<Vertex, Edge>(builder => builder
                    .ConfigureClientFactory(factory => factory
                        .ConfigureClient(client => client)));
        }
    }
}
