// ReSharper disable HeapView.PossibleBoxingAllocation
using System;
using System.Collections.Generic;
using System.Linq;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class UseProviderGremlinQuerySourceTransformation<TProviderConfigurator> : IGremlinQuerySourceTransformation
            where TProviderConfigurator : IProviderConfigurator<TProviderConfigurator>
        {
            private readonly IGremlinqConfiguration _generalSection;
            private readonly IProviderConfiguration _providerSection;
            private readonly ProviderSetupInfo<TProviderConfigurator> _providerSetupInfo;
            private readonly IProviderConfiguratorTransformation<TProviderConfigurator>[] _transformations;

            public UseProviderGremlinQuerySourceTransformation(
                IGremlinqConfiguration generalSection,
                IProviderConfiguration providerSection,
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
                                    options = options.SetValue(WebSocketGremlinqOptions.QueryLogVerbosity, verbosity);

                                if (Enum.TryParse<LogLevel>(loggingSection[$"{nameof(LogLevel)}"], out var logLevel))
                                    options = options.SetValue(WebSocketGremlinqOptions.QueryLogLogLevel, logLevel);

                                if (Enum.TryParse<Formatting>(loggingSection[$"{nameof(Formatting)}"], out var formatting))
                                    options = options.SetValue(WebSocketGremlinqOptions.QueryLogFormatting, formatting);

                                if (Enum.TryParse<GroovyFormatting>(loggingSection[$"{nameof(GroovyFormatting)}"], out var groovyFormatting))
                                    options = options.SetValue(WebSocketGremlinqOptions.QueryLogGroovyFormatting, groovyFormatting);

                                return options;
                            })),
                    configurator =>
                    {
                        if (configurator is IWebSocketProviderConfigurator<TProviderConfigurator> webSocketProviderConfigurator)
                        {
                            configurator = webSocketProviderConfigurator
                                .ConfigureWebSocket(webSocketConfigurator => webSocketConfigurator
                                    .ConfigureFrom(_generalSection)
                                    .ConfigureFrom(_providerSection));
                        }

                        foreach (var transformation in _transformations)
                        {
                            configurator = transformation.Transform(configurator);
                        }

                        return configurator;
                    });
            }
        }

        private sealed class ProviderConfiguration<TConfigurator> : IProviderConfiguration
            where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            private readonly IConfigurationSection _configuration;

            public ProviderConfiguration(IGremlinqConfiguration configuration, ProviderSetupInfo<TConfigurator> setupInfo)
            {
                _configuration = configuration.GetSection(setupInfo.SectionName);
            }

            public IEnumerable<IConfigurationSection> GetChildren() => _configuration.GetChildren();

            public IChangeToken GetReloadToken() => _configuration.GetReloadToken();

            public IConfigurationSection GetSection(string key) => _configuration.GetSection(key);

            public string Key => _configuration.Key;

            public string Path => _configuration.Path;

            public string this[string key]
            {
                get => _configuration[key];
                set => _configuration[key] = value;
            }

            public string Value
            {
                get => _configuration.Value;
                set => _configuration.Value = value;
            }
        }

        private sealed class ProviderSetupInfo<TConfigurator>
            where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            public ProviderSetupInfo(string sectionName, Func<IConfigurableGremlinQuerySource, Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> providerChoice)
            {
                SectionName = sectionName;
                ProviderChoice = providerChoice;
            }

            public string SectionName { get; }
            public Func<IConfigurableGremlinQuerySource, Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> ProviderChoice { get; }
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
                .AddSingleton<IProviderConfiguration, ProviderConfiguration<TConfigurator>>());
        }
    }
}
