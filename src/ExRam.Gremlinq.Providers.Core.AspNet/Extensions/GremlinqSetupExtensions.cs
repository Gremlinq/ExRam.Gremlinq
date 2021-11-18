// ReSharper disable HeapView.PossibleBoxingAllocation
using System;
using System.Collections.Generic;
using System.Linq;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.Core.AspNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class UseProviderGremlinQuerySourceTransformation<TProviderConfigurator> : IGremlinQuerySourceTransformation
            where TProviderConfigurator : IProviderConfigurator<TProviderConfigurator>
        {
            private readonly IGremlinqConfigurationSection _generalSection;
            private readonly IProviderConfigurationSection _providerSection;
            private readonly ProviderSetupInfo<TProviderConfigurator> _providerSetupInfo;
            private readonly IProviderConfiguratorTransformation<TProviderConfigurator>[] _transformations;

            public UseProviderGremlinQuerySourceTransformation(
                IGremlinqConfigurationSection generalSection,
                IProviderConfigurationSection providerSection,
                IEnumerable<IProviderConfiguratorTransformation<TProviderConfigurator>> transformations,
                ProviderSetupInfo<TProviderConfigurator> providerSetupInfo)
            {
                _generalSection = generalSection;
                _providerSection = providerSection;
                _providerSetupInfo = providerSetupInfo;
                _transformations = transformations.ToArray();
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                var loggingSection = _generalSection
                    .GetSection("QueryLogging");

                return _providerSetupInfo.ProviderChoice(
                    source
                        .ConfigureEnvironment(environment => environment
                            .ConfigureOptions(options =>
                            {
                                if (Enum.TryParse<QueryLogVerbosity>(loggingSection["Verbosity"], out var verbosity))
                                    options = options.SetValue(GremlinqOption.QueryLogVerbosity, verbosity);

                                if (Enum.TryParse<LogLevel>(loggingSection[$"{nameof(LogLevel)}"], out var logLevel))
                                    options = options.SetValue(GremlinqOption.QueryLogLogLevel, logLevel);

                                if (Enum.TryParse<Formatting>(loggingSection[$"{nameof(Formatting)}"], out var formatting))
                                    options = options.SetValue(GremlinqOption.QueryLogFormatting, formatting);

                                if (Enum.TryParse<GroovyFormatting>(loggingSection[$"{nameof(GroovyFormatting)}"], out var groovyFormatting))
                                    options = options.SetValue(GremlinqOption.QueryLogGroovyFormatting, groovyFormatting);

                                return options;
                            })),
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
