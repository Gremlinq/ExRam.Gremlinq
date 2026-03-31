using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet.Tests
{
    public class GremlinqConfigurationSectionTests
    {
        private readonly IGremlinqConfigurationSection _section;

        public GremlinqConfigurationSectionTests()
        {
            var serviceCollection = new ServiceCollection()
                .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        { "Gremlinq:Gremlinq_key_1", "value1" },
                        { "Gremlinq:Gremlinq_key_2", "value2" }
                    })
                    .Build())
                .AddGremlinq(_ => { })
                .BuildServiceProvider();

            _section = serviceCollection
                .GetRequiredService<IGremlinqConfigurationSection>();
        }

        [Fact]
        public Task Indexer_can_be_null() => Verify(_section["Key"]);

        [Fact]
        public Task Value_can_be_null() => Verify(_section.Value);

        [Fact]
        public Task General_config() => Verify((
            _section["Gremlinq_key_1"],
            _section["Gremlinq_key_2"]));

        [Fact]
        public void GetChildren_returns_children()
        {
            _section.GetChildren()
                .Should()
                .NotBeNull();
        }

        [Fact]
        public void GetReloadToken_returns_token()
        {
            _section.GetReloadToken()
                .Should()
                .NotBeNull();
        }

        [Fact]
        public void Indexer_set()
        {
            _section["Gremlinq_key_1"] = "updated";

            _section["Gremlinq_key_1"].Should().Be("updated");
        }

        [Fact]
        public void Value_set()
        {
            _section.Value = "some_value";

            _section.Value.Should().Be("some_value");
        }

        [Fact]
        public void Key_returns_key()
        {
            _section.Key.Should().Be("Gremlinq");
        }

        [Fact]
        public void Path_returns_path()
        {
            _section.Path.Should().Be("Gremlinq");
        }

        [Fact]
        public void GetSection_returns_section()
        {
            _section.GetSection("Gremlinq_key_1")
                .Should()
                .NotBeNull();

            _section.GetSection("Gremlinq_key_1").Value
                .Should()
                .Be("value1");
        }

        [Fact]
        public void GetSection_null_throws()
        {
            FluentActions.Invoking(() => _section.GetSection(null!))
                .Should()
                .Throw<ArgumentNullException>();
        }
    }
}
