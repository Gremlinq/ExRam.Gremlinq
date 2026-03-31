using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using ExRam.Gremlinq.Tests.Entities;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Providers.GremlinServer.AspNet.Tests
{
    public class WebSocketConfigurationTests
    {
        [Fact]
        public void With_pool_size() => new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Gremlinq:GremlinServer:Uri", "ws://localhost:8182/" },
                    { "Gremlinq:GremlinServer:ConnectionPool:PoolSize", "4" },
                })
                .Build())
            .AddGremlinq(setup => setup
                .UseGremlinServer<Vertex, Edge>())
            .BuildServiceProvider()
            .GetRequiredService<IGremlinQuerySource>()
            .Should()
            .NotBeNull();

        [Fact]
        public void With_max_in_process_per_connection() => new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Gremlinq:GremlinServer:Uri", "ws://localhost:8182/" },
                    { "Gremlinq:GremlinServer:ConnectionPool:MaxInProcessPerConnection", "16" },
                })
                .Build())
            .AddGremlinq(setup => setup
                .UseGremlinServer<Vertex, Edge>())
            .BuildServiceProvider()
            .GetRequiredService<IGremlinQuerySource>()
            .Should()
            .NotBeNull();

        [Fact]
        public void With_pool_size_and_max_in_process() => new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Gremlinq:GremlinServer:Uri", "ws://localhost:8182/" },
                    { "Gremlinq:GremlinServer:ConnectionPool:PoolSize", "4" },
                    { "Gremlinq:GremlinServer:ConnectionPool:MaxInProcessPerConnection", "16" },
                })
                .Build())
            .AddGremlinq(setup => setup
                .UseGremlinServer<Vertex, Edge>())
            .BuildServiceProvider()
            .GetRequiredService<IGremlinQuerySource>()
            .Should()
            .NotBeNull();

        [Fact]
        public void Without_uri() => new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>())
                .Build())
            .AddGremlinq(setup => setup
                .UseGremlinServer<Vertex, Edge>())
            .BuildServiceProvider()
            .GetRequiredService<IGremlinQuerySource>()
            .Should()
            .NotBeNull();

        [Fact]
        public void With_authentication_without_password() => new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Gremlinq:GremlinServer:Authentication:Username", "user" },
                })
                .Build())
            .AddGremlinq(setup => setup
                .UseGremlinServer<Vertex, Edge>())
            .BuildServiceProvider()
            .GetRequiredService<IGremlinQuerySource>()
            .Should()
            .NotBeNull();

        [Fact]
        public void With_authentication_without_username() => new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Gremlinq:GremlinServer:Authentication:Password", "pass" },
                })
                .Build())
            .AddGremlinq(setup => setup
                .UseGremlinServer<Vertex, Edge>())
            .BuildServiceProvider()
            .GetRequiredService<IGremlinQuerySource>()
            .Should()
            .NotBeNull();
    }
}
