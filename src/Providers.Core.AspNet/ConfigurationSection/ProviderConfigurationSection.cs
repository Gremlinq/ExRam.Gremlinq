// ReSharper disable HeapView.PossibleBoxingAllocation
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.Core.AspNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace ExRam.Gremlinq.Core.AspNet
{
    internal sealed class ProviderConfigurationSection<TConfigurator> : IProviderConfigurationSection
        where TConfigurator : IProviderConfigurator<TConfigurator>
    {
        private readonly IConfigurationSection _providerSection;
        private readonly IGremlinqConfigurationSection _gremlinqSection;

        public ProviderConfigurationSection(IGremlinqConfigurationSection gremlinqSection, string sectionName)
        {
            _gremlinqSection = gremlinqSection;
            _providerSection = gremlinqSection
                .GetSection(sectionName);
        }

        IEnumerable<IConfigurationSection> IConfiguration.GetChildren() => _providerSection.GetChildren();

        IChangeToken IConfiguration.GetReloadToken() => _providerSection.GetReloadToken();

        IConfigurationSection IConfiguration.GetSection(string key) => _providerSection.GetSection(key);

        IGremlinqConfigurationSection IProviderConfigurationSection.GremlinqSection => _gremlinqSection;

        string IConfigurationSection.Key => _providerSection.Key;

        string IConfigurationSection.Path => _providerSection.Path;

        string? IConfiguration.this[string key]
        {
            get => _providerSection[key];
            set => _providerSection[key] = value;
        }

        string? IConfigurationSection.Value
        {
            get => _providerSection.Value;
            set => _providerSection.Value = value;
        }
    }
}
