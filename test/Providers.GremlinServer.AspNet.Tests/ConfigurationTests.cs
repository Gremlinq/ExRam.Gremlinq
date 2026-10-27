using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using ExRam.Gremlinq.Tests.Entities;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Providers.GremlinServer.AspNet.Tests
{
    public class ConfigurationTests
    {
        [Fact]
        public void Source_can_be_created() => new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Gremlinq:GremlinServer:Uri", "ws://localhost:8182/" },
                })
                .Build())
            .AddGremlinq(setup => setup
                .UseGremlinServer<Vertex, Edge>())
            .BuildServiceProvider()
            .GetRequiredService<IGremlinQuerySource>()
            .Should()
            .NotBeNull();

        [Fact]
        public void With_alias() => new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Gremlinq:Alias", "a" },
                })
                .Build())
            .AddGremlinq(setup => setup
                .UseGremlinServer<Vertex, Edge>())
            .BuildServiceProvider()
            .GetRequiredService<IGremlinQuerySource>()
            .AsAdmin()
            .Environment
            .Options
            .GetValue(GremlinqOption.Alias)
            .Should()
            .Be("a");

        [Fact]
        public void With_authentication() => new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Gremlinq:GremlinServer:Authentication:Username", "user" },
                    { "Gremlinq:GremlinServer:Authentication:Password", "pass" }
                })
                .Build())
            .AddGremlinq(setup => setup
                .UseGremlinServer<Vertex, Edge>())
            .BuildServiceProvider()
            .GetRequiredService<IGremlinQuerySource>()
            .Should()
            .NotBeNull();

        [Fact]
        public void FromBaseSection_after_provider() => new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Gremlinq:Alias", "a" },
                    { "MyApp:Gremlinq:Alias", "b" }
                })
                .Build())
            .AddGremlinq(setup => setup
                .UseGremlinServer<Vertex, Edge>()
                .FromBaseSection("MyApp"))
            .BuildServiceProvider()
            .GetRequiredService<IGremlinQuerySource>()
            .AsAdmin()
            .Environment
            .Options
            .GetValue(GremlinqOption.Alias)
            .Should()
            .Be("b");

        [Fact]
        public void ConfigureQuerySource_generic_after_provider() => new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .Build())
            .AddGremlinq(setup => setup
                .UseGremlinServer<Vertex, Edge>()
                .ConfigureQuerySource<AliasTransformation>())
            .BuildServiceProvider()
            .GetRequiredService<IGremlinQuerySource>()
            .AsAdmin()
            .Environment
            .Options
            .GetValue(GremlinqOption.Alias)
            .Should()
            .Be("c");

        private sealed class AliasTransformation : IGremlinQuerySourceTransformation
        {
            public IGremlinQuerySource Transform(IGremlinQuerySource source) => source
                .ConfigureEnvironment(environment => environment
                    .ConfigureOptions(options => options
                        .SetValue(GremlinqOption.Alias, "c")));
        }
    }
}
