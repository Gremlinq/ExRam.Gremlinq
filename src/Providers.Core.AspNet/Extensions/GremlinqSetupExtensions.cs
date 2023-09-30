// ReSharper disable HeapView.PossibleBoxingAllocation
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.Core.AspNet;

using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class UseProviderGremlinQuerySourceTransformation<TProviderConfigurator> : IGremlinQuerySourceTransformation
            where TProviderConfigurator : IProviderConfigurator<TProviderConfigurator>
        {
            private readonly IProviderConfigurationSection _section;
            private readonly ProviderSetupInfo<TProviderConfigurator> _providerSetupInfo;

            public UseProviderGremlinQuerySourceTransformation(
                IProviderConfigurationSection section,
                ProviderSetupInfo<TProviderConfigurator> providerSetupInfo)
            {
                _section = section;
                _providerSetupInfo = providerSetupInfo;
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source) => _providerSetupInfo.ProviderChoice(source, _section);
        }

        public static ProviderSetup<TConfigurator> UseProvider<TConfigurator>(
            this GremlinqSetup setup,
            string sectionName,
            Func<IConfigurableGremlinQuerySource, IProviderConfigurationSection, IGremlinQuerySource> providerChoice)
                where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            setup.ServiceCollection
                .AddSingleton(new ProviderSetupInfo<TConfigurator>(sectionName, providerChoice))
                .AddSingleton<IGremlinQuerySourceTransformation, UseProviderGremlinQuerySourceTransformation<TConfigurator>>()
                .AddSingleton<IProviderConfigurationSection, ProviderConfigurationSection<TConfigurator>>();

            return new ProviderSetup<TConfigurator>(setup.ServiceCollection);
        }
    }
}
