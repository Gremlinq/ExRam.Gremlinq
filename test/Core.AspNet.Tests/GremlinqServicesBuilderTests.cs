using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core.AspNet.Tests
{
    public class GremlinqServicesBuilderTests
    {
        [Fact]
        public void ConfigureQuerySource_with_section()
        {
            var source = new ServiceCollection()
                .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        { "Gremlinq:Alias", "g" }
                    })
                    .Build())
                .AddGremlinq(builder => builder
                    .ConfigureQuerySource((source, section) => source))
                .BuildServiceProvider()
                .GetRequiredService<IGremlinQuerySource>();

            source.Should().NotBeNull();
        }

        [Fact]
        public void FromBaseSection()
        {
            var section = new ServiceCollection()
                .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        { "MyApp:Gremlinq:key1", "value1" }
                    })
                    .Build())
                .AddGremlinq(builder => builder
                    .FromBaseSection("MyApp"))
                .BuildServiceProvider()
                .GetRequiredService<IGremlinqConfigurationSection>();

            section["key1"].Should().Be("value1");
        }

        [Fact]
        public void FromBaseSection_overrides_default()
        {
            var section = new ServiceCollection()
                .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        { "Gremlinq:defaultKey", "defaultValue" },
                        { "CustomSection:Gremlinq:customKey", "customValue" }
                    })
                    .Build())
                .AddGremlinq(builder => builder
                    .FromBaseSection("CustomSection"))
                .BuildServiceProvider()
                .GetRequiredService<IGremlinqConfigurationSection>();

            section["customKey"].Should().Be("customValue");
            section["defaultKey"].Should().BeNull();
        }

        [Fact]
        public void ConfigureQuerySource_generic()
        {
            var source = new ServiceCollection()
                .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string?>())
                    .Build())
                .AddGremlinq(builder => builder
                    .ConfigureQuerySource<NoOpTransformation>())
                .BuildServiceProvider()
                .GetRequiredService<IGremlinQuerySource>();

            source.Should().NotBeNull();
        }

        [Fact]
        public void QueryLogging()
        {
            var options = new ServiceCollection()
                .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        { "Gremlinq:QueryLogging:Verbosity", "IncludeBindings" },
                        { "Gremlinq:QueryLogging:LogLevel", "Warning" }
                    })
                    .Build())
                .AddGremlinq(_ => { })
                .BuildServiceProvider()
                .GetRequiredService<IGremlinQuerySource>()
                .AsAdmin()
                .Environment
                .Options;

            options.GetValue(GremlinqOption.QueryLogVerbosity).Should().Be(QueryLogVerbosity.IncludeBindings);
            options.GetValue(GremlinqOption.QueryLogLogLevel).Should().Be(LogLevel.Warning);
        }

        private class NoOpTransformation : IGremlinQuerySourceTransformation
        {
            public IGremlinQuerySource Transform(IGremlinQuerySource source) => source;
        }
    }
}
