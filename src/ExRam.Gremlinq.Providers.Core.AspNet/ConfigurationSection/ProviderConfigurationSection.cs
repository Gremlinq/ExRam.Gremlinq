// ReSharper disable HeapView.PossibleBoxingAllocation
using System.Collections.Generic;
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

        public ProviderConfigurationSection(IGremlinqConfigurationSection configuration, ProviderSetupInfo<TConfigurator> setupInfo)
        {
            _providerSection = configuration.GetSection(setupInfo.SectionName);
        }

        IEnumerable<IConfigurationSection> IConfiguration.GetChildren() => _providerSection.GetChildren();

        IChangeToken IConfiguration.GetReloadToken() => _providerSection.GetReloadToken();

        IConfigurationSection IConfiguration.GetSection(string key) => _providerSection.GetSection(key);

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
