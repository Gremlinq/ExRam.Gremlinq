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

        public ProviderConfigurationSectionTests() : base()
        {

        }

        [Fact]
        public void Indexer_can_be_null()
        {
            var serviceCollection = new ServiceCollection()
                .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                    .AddInMemoryCollection()
                    .Build())
                .AddGremlinq(s => s
                    .UseProvider<IMyProviderConfigurator>(
                        "Provider",
                        (source, _) => source
                            .ConfigureEnvironment(_ => _),
                        setup => { },
                        default!))
                .BuildServiceProvider();

            var section = serviceCollection
                .GetRequiredService<IProviderConfigurationSection>();

            section["Key"]
                .Should()
                .BeNull();
        }
    }
}
