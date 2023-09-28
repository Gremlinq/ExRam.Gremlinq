using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using ExRam.Gremlinq.Tests.Entities;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Providers.JanusGraph.AspNet.Tests
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
                        { "Gremlinq:JanusGraph:Uri", "ws://localhost:8182/" },
                    })
                    .Build())
                .AddGremlinq(setup => setup
                    .UseJanusGraph<Vertex, Edge>())
                .BuildServiceProvider()
                .GetRequiredService<IGremlinQuerySource>()
                .Should()
                .NotBeNull();
        }
    }
}
