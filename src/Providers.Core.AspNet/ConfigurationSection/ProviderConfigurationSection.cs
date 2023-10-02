// ReSharper disable HeapView.PossibleBoxingAllocation
using ExRam.Gremlinq.Providers.Core.AspNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace ExRam.Gremlinq.Core.AspNet
{
    internal sealed class ProviderConfigurationSection<TConfigurator> : IProviderConfigurationSection
        where TConfigurator : IGremlinqConfigurator<TConfigurator>
    {
        private readonly IConfigurationSection _baseSection;

        public ProviderConfigurationSection(IGremlinqConfigurationSection gremlinqSection) : this((IConfigurationSection)gremlinqSection)
        {

        }

        public ProviderConfigurationSection(IGremlinqConfigurationSection gremlinqSection, string sectionName) : this(gremlinqSection.GetSection(sectionName))
        {

        }

        private ProviderConfigurationSection(IConfigurationSection section)
        {
            _baseSection = section;
        }

        IEnumerable<IConfigurationSection> IConfiguration.GetChildren() => _baseSection.GetChildren();

        IChangeToken IConfiguration.GetReloadToken() => _baseSection.GetReloadToken();

        IConfigurationSection IConfiguration.GetSection(string key) => _baseSection.GetSection(key);

        string IConfigurationSection.Key => _baseSection.Key;

        string IConfigurationSection.Path => _baseSection.Path;

        string? IConfiguration.this[string key]
        {
            get => _baseSection[key];
            set => _baseSection[key] = value;
        }

        string? IConfigurationSection.Value
        {
            get => _baseSection.Value;
            set => _baseSection.Value = value;
        }
    }
}
