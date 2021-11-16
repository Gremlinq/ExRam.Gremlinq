using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using NullGuard;

namespace ExRam.Gremlinq.Core.AspNet
{
    internal sealed class GremlinqConfigurationSection : IGremlinqConfigurationSection
    {
        private readonly IConfigurationSection _baseConfiguration;

        public GremlinqConfigurationSection(IConfigurationSection baseConfiguration)
        {
            _baseConfiguration = baseConfiguration;
        }

        IEnumerable<IConfigurationSection> IConfiguration.GetChildren() => _baseConfiguration.GetChildren();

        IChangeToken IConfiguration.GetReloadToken() => _baseConfiguration.GetReloadToken();

        IConfigurationSection IConfiguration.GetSection(string key) => _baseConfiguration.GetSection(key);

        string? IConfiguration.this[string key]
        {
            get => _baseConfiguration[key];
            set => _baseConfiguration[key] = value;
        }

        string IConfigurationSection.Value
        {
            get => _baseConfiguration.Value;
            set => _baseConfiguration.Value = value;
        }

        string IConfigurationSection.Key => _baseConfiguration.Key;

        string IConfigurationSection.Path => _baseConfiguration.Path;
    }
}
