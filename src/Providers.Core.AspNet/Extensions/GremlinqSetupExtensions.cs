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
            private readonly ProviderSetupInfo<TProviderConfigurator> _providerSetupInfo;
            private readonly IEnumerable<IProviderConfiguratorTransformation<TProviderConfigurator>> _providerConfiguratorTransformations;

            public UseProviderGremlinQuerySourceTransformation(
                ProviderSetupInfo<TProviderConfigurator> providerSetupInfo,
                IEnumerable<IProviderConfiguratorTransformation<TProviderConfigurator>> providerConfiguratorTransformations)
            {
                _providerSetupInfo = providerSetupInfo;
                _providerConfiguratorTransformations = providerConfiguratorTransformations;
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source) => _providerSetupInfo.ProviderChoice(
                source,
                configurator =>
                {
                    foreach (var transformation in _providerConfiguratorTransformations)
                    {
                        configurator = transformation.Transform(configurator);
                    }

                    return configurator;
                });
        }

        public static ProviderSetup<TConfigurator> UseProvider<TConfigurator>(
            this GremlinqSetup setup,
            string sectionName,
            Func<IConfigurableGremlinQuerySource, Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> providerChoice)
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
