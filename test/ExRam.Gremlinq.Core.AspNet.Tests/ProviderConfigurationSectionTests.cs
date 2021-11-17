using System.Collections.Generic;
using ExRam.Gremlinq.Providers.Core;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ExRam.Gremlinq.Core.AspNet.Tests
{
    public class ProviderConfigurationSectionTests
    {
        private interface IMyProviderConfigurator : IProviderConfigurator<IMyProviderConfigurator>
        {

        }

        private readonly IProviderConfigurationSection _section;

        public ProviderConfigurationSectionTests() : base()
        {
            var serviceCollection = new ServiceCollection()
                .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "Gremlinq:Gremlinq_key_1", "value1" },
                        { "Gremlinq:Gremlinq_key_2", "value2" },

                        { "Gremlinq:Provider1:Provider1_key_1", "provider1_value1" },
                        { "Gremlinq:Provider1:Provider1_key_2", "provider1_value2" },

                        { "Gremlinq:Provider2:Provider2_key_1", "provider2_value1" },
                        { "Gremlinq:Provider2:Provider2_key_2", "provider2_value2" }
                   })
                   .Build())
                .AddGremlinq(s => s
                    .UseProvider<IMyProviderConfigurator>(
                        "Provider1",
                        (source, _) => source
                            .ConfigureEnvironment(_ => _),
                        setup => { },
                        default!))
                .BuildServiceProvider();

            _section = serviceCollection
                .GetRequiredService<IProviderConfigurationSection>();
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
        public void Provider_config()
        {
            _section["Provider1_key_1"]
                .Should()
                .Be("provider1_value1");

            _section["Provider1_key_2"]
                .Should()
                .Be("provider1_value2");

            _section["Provider2_key_1"]
              .Should()
              .BeNull();

            _section["Provider2_key_2"]
                .Should()
                .BeNull();
        }
    }
}
