using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace ExRam.Gremlinq.Core.AspNet
{
    internal sealed class GremlinqConfigurationSection : IGremlinqConfigurationSection
    {
        private readonly IConfigurationSection _baseConfiguration;

        public GremlinqConfigurationSection(IConfigurationSection baseConfiguration)
        {
            _baseConfiguration = baseConfiguration;
        }

        public IEnumerable<IConfigurationSection> GetChildren() => _baseConfiguration.GetChildren();

        public IChangeToken GetReloadToken() => _baseConfiguration.GetReloadToken();

        public IConfigurationSection GetSection(string key) => _baseConfiguration.GetSection(key);

        public string? this[string key]
        {
            get => _baseConfiguration[key];
            set => _baseConfiguration[key] = value;
        }

        public string Key => _baseConfiguration.Key;

        public string Path => _baseConfiguration.Path;

        public string Value { get => _baseConfiguration.Value; set => _baseConfiguration.Value = value; }
    }
}
