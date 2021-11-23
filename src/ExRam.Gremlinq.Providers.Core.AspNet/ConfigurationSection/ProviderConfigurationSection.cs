// ReSharper disable HeapView.PossibleBoxingAllocation
using System;
using System.Collections.Generic;
using System.Linq;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.Core.AspNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace ExRam.Gremlinq.Core.AspNet
{
    internal sealed class ProviderConfigurationSection<TConfigurator> : IProviderConfigurationSection
        where TConfigurator : IProviderConfigurator<TConfigurator>
    {
        private sealed class MergedProviderConfigurationSection : IProviderConfigurationSection
        {
            private readonly IProviderConfigurationSection _providerSection;
            private readonly IGremlinqConfigurationSection _gremlinqSection;

            public MergedProviderConfigurationSection(IProviderConfigurationSection providerSection, IGremlinqConfigurationSection gremlinqSection)
            {
                _providerSection = providerSection;
                _gremlinqSection = gremlinqSection;
            }

            string? IConfiguration.this[string key]
            {
                get => _providerSection[key] ?? _gremlinqSection[key];
                set => _providerSection[key] = value;
            }

            string IConfigurationSection.Key => _providerSection.Key ?? _gremlinqSection.Key;

            string IConfigurationSection.Path => _providerSection.Path ?? _gremlinqSection.Path;

            string? IConfigurationSection.Value
            {
                get => _providerSection.Value ?? _gremlinqSection.Value;
                set => _providerSection.Value = value;
            }

            IEnumerable<IConfigurationSection> IConfiguration.GetChildren() => _providerSection
                .GetChildren()
                .Concat(_gremlinqSection
                    .GetChildren()
                    .Where(section => section.Path != _providerSection.Path));

            IChangeToken IConfiguration.GetReloadToken() => throw new NotImplementedException($"Cannot call {nameof(IConfiguration.GetReloadToken)} on a merged {nameof(IProviderConfigurationSection)}.");

            IConfigurationSection IConfiguration.GetSection(string key) => _providerSection.GetSection(key);

            IProviderConfigurationSection IProviderConfigurationSection.MergeWithGremlinqSection() => this;
        }

        private readonly IConfigurationSection _providerSection;
        private readonly IGremlinqConfigurationSection _gremlinqSection;

        public ProviderConfigurationSection(IGremlinqConfigurationSection gremlinqSection, ProviderSetupInfo<TConfigurator> setupInfo)
        {
            _gremlinqSection = gremlinqSection;
            _providerSection = gremlinqSection
                .GetSection(setupInfo.SectionName);
        }

        IEnumerable<IConfigurationSection> IConfiguration.GetChildren() => _providerSection.GetChildren();

        IChangeToken IConfiguration.GetReloadToken() => _providerSection.GetReloadToken();

        IConfigurationSection IConfiguration.GetSection(string key) => _providerSection.GetSection(key);

        IProviderConfigurationSection IProviderConfigurationSection.MergeWithGremlinqSection() => new MergedProviderConfigurationSection(this, _gremlinqSection);

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
