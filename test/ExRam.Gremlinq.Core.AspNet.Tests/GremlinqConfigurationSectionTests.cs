using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VerifyXunit;
using Xunit;

namespace ExRam.Gremlinq.Core.AspNet.Tests
{
    public class GremlinqConfigurationSectionTests : VerifyBase
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
        public Task Indexer_can_be_null() => Verify(_section["Key"]);

        [Fact]
        public Task Value_can_be_null() => Verify(_section.Value);

        [Fact]
        public Task General_config() => Verify((
            _section["Gremlinq_key_1"],
            _section["Gremlinq_key_2"]));
    }
}
