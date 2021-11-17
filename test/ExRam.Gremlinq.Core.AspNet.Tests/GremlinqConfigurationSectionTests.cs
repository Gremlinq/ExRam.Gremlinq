using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ExRam.Gremlinq.Core.AspNet.Tests
{
    public class GremlinqConfigurationSectionTests
    {
        private readonly IGremlinqConfigurationSection _section;

        public GremlinqConfigurationSectionTests() : base()
        {
            var serviceCollection = new ServiceCollection()
                .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                    .AddInMemoryCollection()
                    .Build())
                .AddGremlinq(s => { })
                .BuildServiceProvider();

            _section = serviceCollection
                .GetRequiredService<IGremlinqConfigurationSection>();
        }

        [Fact]
        public void Indexer_can_be_null()
        {
            _section["Key"]
                .Should()
                .BeNull();
        }
    }
}
