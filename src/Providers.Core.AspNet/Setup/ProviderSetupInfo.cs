using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.Core.AspNet;

namespace ExRam.Gremlinq.Core.AspNet
{
    internal sealed class ProviderSetupInfo<TConfigurator>
        where TConfigurator : IProviderConfigurator<TConfigurator>
    {
        public ProviderSetupInfo(string sectionName, Func<IConfigurableGremlinQuerySource, IProviderConfigurationSection, IGremlinQuerySource> providerChoice)
        {
            SectionName = sectionName;
            ProviderChoice = providerChoice;
        }

        public string SectionName { get; }
        public Func<IConfigurableGremlinQuerySource, IProviderConfigurationSection, IGremlinQuerySource> ProviderChoice { get; }
    }
}
