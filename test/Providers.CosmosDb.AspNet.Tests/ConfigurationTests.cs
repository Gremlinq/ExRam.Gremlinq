using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Tests.Entities;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Providers.CosmosDb.AspNet.Tests
{
    public class ConfigurationTests
    {
        [Fact]
        public void Source_can_be_created()
        {
            new ServiceCollection()
                .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        { "Gremlinq:CosmosDb:Uri", "wss://your_cosmosdb-endpoint.gremlin.cosmos.azure.com:443/" },
                        { "Gremlinq:CosmosDb:Database", "db" },
                        { "Gremlinq:CosmosDb:Graph", "collection" },
                        { "Gremlinq:CosmosDb:AuthKey", "yourAuthKey" }
                    })
                    .Build())
                .AddGremlinq(setup => setup
                    .UseCosmosDb<Vertex, Edge>((conf, section) => conf
                        .WithPartitionKey(x => x.Label!)))
                .BuildServiceProvider()
                .GetRequiredService<IGremlinQuerySource>()
                .Should()
                .NotBeNull();
        }
    }
}
