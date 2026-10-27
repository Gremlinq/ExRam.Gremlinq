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
        public void Source_can_be_created() => new ServiceCollection()
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

        [Fact]
        public void With_authentication() => new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Gremlinq:JanusGraph:Authentication:Username", "user" },
                    { "Gremlinq:JanusGraph:Authentication:Password", "pass" }
                })
                .Build())
            .AddGremlinq(setup => setup
                .UseJanusGraph<Vertex, Edge>())
            .BuildServiceProvider()
            .GetRequiredService<IGremlinQuerySource>()
            .Should()
            .NotBeNull();

        [Fact]
        public void ServerCompatibilityRange() => new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Gremlinq:ServerCompatibilityRange:Lowest", "3.7.7" },
                    { "Gremlinq:ServerCompatibilityRange:Highest", "3.8.2" }
                })
                .Build())
            .AddGremlinq(setup => setup
                .UseJanusGraph<Vertex, Edge>())
            .BuildServiceProvider()
            .GetRequiredService<IGremlinQuerySource>()
            .AsAdmin()
            .Environment
            .ServerCompatibilityRange
            .Should()
            .Be(VersionRange.From(TinkerPopVersions.V3_7_7, TinkerPopVersions.V3_8_2));
    }
}
