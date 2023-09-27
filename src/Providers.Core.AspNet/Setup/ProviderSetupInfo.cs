using ExRam.Gremlinq.Providers.Core;

using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    internal sealed class ProviderSetupInfo<TConfigurator>
        where TConfigurator : IProviderConfigurator<TConfigurator>
    {
        public ProviderSetupInfo(string sectionName, Func<IConfigurableGremlinQuerySource, IConfigurationSection, IGremlinQuerySource> providerChoice)
        {
            SectionName = sectionName;
            ProviderChoice = providerChoice;
        }

        public string SectionName { get; }
        public Func<IConfigurableGremlinQuerySource, IConfigurationSection, IGremlinQuerySource> ProviderChoice { get; }
    }
}
