using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace ExRam.Gremlinq.Core.AspNet
{
    internal sealed class GremlinqConfiguration : IGremlinqConfiguration
    {
        private readonly IConfiguration _baseConfiguration;

        public GremlinqConfiguration(IConfiguration baseConfiguration)
        {
            _baseConfiguration = baseConfiguration;
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return _baseConfiguration.GetChildren();
        }

        public IChangeToken GetReloadToken()
        {
            return _baseConfiguration.GetReloadToken();
        }

        public IConfigurationSection GetSection(string key)
        {
            return _baseConfiguration.GetSection(key);
        }

        public string this[string key]
        {
            get => _baseConfiguration[key];
            set => _baseConfiguration[key] = value;
        }
    }
}