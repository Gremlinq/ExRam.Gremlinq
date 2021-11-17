using System.Collections.Generic;
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
                    .AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "Gremlinq:Gremlinq_key_1", "value1" },
                        { "Gremlinq:Gremlinq_key_2", "value2" }
                    })
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

        [Fact]
        public void Value_can_be_null()
        {
            _section
                .Value
                .Should()
                .BeNull();
        }

        [Fact]
        public void General_config()
        {
            _section["Gremlinq_key_1"]
                .Should()
                .Be("value1");

            _section["Gremlinq_key_2"]
                .Should()
                .Be("value2");
        }
    }
}
