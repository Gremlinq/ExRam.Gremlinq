// ReSharper disable HeapView.PossibleBoxingAllocation
using System;
using System.Collections.Generic;
using System.Linq;
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
            private readonly IProviderConfiguratorTransformation<TProviderConfigurator>[] _transformations;

            public UseProviderGremlinQuerySourceTransformation(
                IEnumerable<IProviderConfiguratorTransformation<TProviderConfigurator>> transformations,
                ProviderSetupInfo<TProviderConfigurator> providerSetupInfo)
            {
                _providerSetupInfo = providerSetupInfo;
                _transformations = transformations.ToArray();
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                return _providerSetupInfo.ProviderChoice(
                    source,
                    configurator =>
                    {
                        foreach (var transformation in _transformations)
                        {
                            configurator = transformation.Transform(configurator);
                        }

                        return configurator;
                    });
            }
        }

        public static GremlinqSetup UseProvider<TConfigurator>(
            this GremlinqSetup setup,
            string sectionName,
            Func<IConfigurableGremlinQuerySource, Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> providerChoice,
            Action<ProviderSetup<TConfigurator>> setupAction,
            Action<ProviderSetup<TConfigurator>>? extraSetupAction) where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            setupAction(new ProviderSetup<TConfigurator>(setup.ServiceCollection));

            if (extraSetupAction is { } extraConfiguration)
                extraConfiguration(new ProviderSetup<TConfigurator>(setup.ServiceCollection));

            return setup.RegisterTypes(serviceCollection => serviceCollection
                .AddSingleton(new ProviderSetupInfo<TConfigurator>(sectionName, providerChoice))
                .AddSingleton<IGremlinQuerySourceTransformation, UseProviderGremlinQuerySourceTransformation<TConfigurator>>()
                .AddSingleton<IProviderConfigurationSection, ProviderConfigurationSection<TConfigurator>>());
        }
    }
}
