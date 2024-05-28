using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
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
                        { "Gremlinq:CosmosDb:Uri", "ws://localhost:8182/" },
                        { "Gremlinq:CosmosDb:Database", "db" },
                        { "Gremlinq:CosmosDb:Graph", "collection" },
                        { "Gremlinq:CosmosDb:AuthKey", "yourAuthKey" }
                    })
                    .Build())
                .AddGremlinq(setup => setup
                    .UseCosmosDb<Vertex, Edge>()
                    .Configure((conf, section) => conf
                        .WithPartitionKey(x => x.Label!)))
                .BuildServiceProvider()
                .GetRequiredService<IGremlinQuerySource>()
                .Should()
                .NotBeNull();
        }

        [Fact]
        public void Configuration_without_PartitionKey()
        {
            new ServiceCollection()
                .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        { "Gremlinq:CosmosDb:Uri", "ws://localhost:8182/" },
                        { "Gremlinq:CosmosDb:Database", "db" },
                        { "Gremlinq:CosmosDb:Graph", "collection" },
                        { "Gremlinq:CosmosDb:AuthKey", "yourAuthKey" }
                    })
                    .Build())
                .AddGremlinq(setup => setup
                    .UseCosmosDb<Vertex, Edge>())
                .BuildServiceProvider()
                .Invoking(_ => _
                    .GetRequiredService<IGremlinQuerySource>())
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void Configuration_with_PartitionKey_in_configuration()
        {
            new ServiceCollection()
                .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        { "Gremlinq:CosmosDb:Uri", "ws://localhost:8182/" },
                        { "Gremlinq:CosmosDb:Database", "db" },
                        { "Gremlinq:CosmosDb:Graph", "collection" },
                        { "Gremlinq:CosmosDb:AuthKey", "yourAuthKey" },
                        { "Gremlinq:CosmosDb:PartitionKey", "PartitionKey" }    //TODO: Test when Property is defined on base class
                    })
                    .Build())
                .AddGremlinq(setup => setup
                    .UseCosmosDb<Vertex, Edge>())
                .BuildServiceProvider()
                .GetRequiredService<IGremlinQuerySource>()
                .Should()
                .NotBeNull();
        }
    }
}
