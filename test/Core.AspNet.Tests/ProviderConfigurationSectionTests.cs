using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.Core.AspNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet.Tests
{
    public class ProviderConfigurationSectionTests : VerifyBase
    {
        private readonly IConfigurationRoot _configurationRoot;

        private interface IMyProviderConfigurator : IProviderConfigurator<IMyProviderConfigurator>
        {
            
        }

        public ProviderConfigurationSectionTests() : base()
        {
            _configurationRoot = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Gremlinq:Gremlinq_key_1", "value1" },
                    { "Gremlinq:Gremlinq_key_2", "value2" },

                    { "Gremlinq:Provider1:Provider1_key_1", "provider1_value1" },
                    { "Gremlinq:Provider1:Provider1_key_2", "provider1_value2" },

                    { "Gremlinq:Provider2:Provider2_key_1", "provider2_value1" },
                    { "Gremlinq:Provider2:Provider2_key_2", "provider2_value2" }
               })
               .Build();
        }

        [Fact]
        public Task Indexer_can_be_null()
        {
            var section = new ServiceCollection()
                .AddSingleton<IConfiguration>(_configurationRoot)
                .AddGremlinq(s => s
                    .UseProvider<IMyProviderConfigurator>(
                        source => _ => source
                            .ConfigureEnvironment(_ => _))
                    .FromProviderSection("Provider1"))
                .BuildServiceProvider()
                .GetRequiredService<IProviderConfigurationSection>();

            return Verify(section["Key"]);
        }

        [Fact]
        public Task Value_can_be_null()
        {
            var section = new ServiceCollection()
                .AddSingleton<IConfiguration>(_configurationRoot)
                .AddGremlinq(s => s
                    .UseProvider<IMyProviderConfigurator>(
                        source => _ => source
                            .ConfigureEnvironment(_ => _))
                    .FromProviderSection("Provider1"))
                .BuildServiceProvider()
                .GetRequiredService<IProviderConfigurationSection>();

            return Verify(section.Value);
        }

        [Fact]
        public Task Provider_config()
        {
            var section = new ServiceCollection()
                .AddSingleton<IConfiguration>(_configurationRoot)
                .AddGremlinq(s => s
                    .UseProvider<IMyProviderConfigurator>(
                        source => _ => source
                            .ConfigureEnvironment(_ => _))
                    .FromProviderSection("Provider1"))
                .BuildServiceProvider()
                .GetRequiredService<IProviderConfigurationSection>();

            return Verify((
                section["Gremlinq_key_1"],
                section["Gremlinq_key_2"],

                section["Provider1_key_1"],
                section["Provider1_key_2"],
                section["Provider2_key_1"],
                section["Provider2_key_2"]));
        }

        [Fact]
        public Task Provider_config_without_explicit_provider_section()
        {
            var section = new ServiceCollection()
                .AddSingleton<IConfiguration>(_configurationRoot)
                .AddGremlinq(s => s
                    .UseProvider<IMyProviderConfigurator>(
                        source => _ => source
                            .ConfigureEnvironment(_ => _)))
                .BuildServiceProvider()
                .GetRequiredService<IProviderConfigurationSection>();

            return Verify((
                section["Gremlinq_key_1"],
                section["Gremlinq_key_2"],

                section["Provider1_key_1"],
                section["Provider1_key_2"],
                section["Provider2_key_1"],
                section["Provider2_key_2"]));
        }
    }
}
