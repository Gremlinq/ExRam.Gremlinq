using ExRam.Gremlinq.Core.AspNet;

using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Providers.Core.AspNet
{
    public interface IProviderConfigurationSection : IConfigurationSection
    {
        IGremlinqConfigurationSection GremlinqSection { get; }
    }
}
