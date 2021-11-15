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
            private readonly IProviderConfiguratorTransformation<TProviderConfigurator>[] _transformations;
            private readonly Func<IConfigurableGremlinQuerySource, Func<TProviderConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> _providerChoice;
            
            public UseProviderGremlinQuerySourceTransformation(
                IGremlinqConfiguration generalSection,
                IProviderConfiguration providerSection,
                IEnumerable<IProviderConfiguratorTransformation<TProviderConfigurator>> transformations,
                Func<IConfigurableGremlinQuerySource, Func<TProviderConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> providerChoice)
            {
                _generalSection = generalSection;
                _providerSection = providerSection;
                _providerChoice = providerChoice;
                _transformations = transformations.ToArray();
            }
            
            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                var loggingSection = _generalSection
                    .GetSection("QueryLogging");

                return _providerChoice(
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

        private sealed class ProviderConfiguration : IProviderConfiguration
        {
            private readonly IConfiguration _configuration;

            public ProviderConfiguration(IGremlinqConfiguration configuration, string sectionName)
            {
                _configuration = configuration.GetSection(sectionName);
            }

            public string this[string key] { get => _configuration[key]; set => _configuration[key] = value; }

            public IEnumerable<IConfigurationSection> GetChildren() => _configuration.GetChildren();

            public IChangeToken GetReloadToken() => _configuration.GetReloadToken();

            public IConfigurationSection GetSection(string key) => _configuration.GetSection(key);
        }

        public static GremlinqSetup UseProvider<TConfigurator>(
            this GremlinqSetup setup,
            string sectionName,
            Func<IConfigurableGremlinQuerySource, Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> providerChoice) where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            return setup.RegisterTypes(serviceCollection => serviceCollection
                .AddSingleton<IGremlinQuerySourceTransformation>(s => new UseProviderGremlinQuerySourceTransformation<TConfigurator>(
                    s.GetRequiredService<IGremlinqConfiguration>(),
                    s.GetRequiredService<IProviderConfiguration>(),
                    s.GetRequiredService<IEnumerable<IProviderConfiguratorTransformation<TConfigurator>>>(),
                    providerChoice))
                .AddSingleton<IProviderConfiguration>(s => new ProviderConfiguration(
                    s.GetRequiredService<IGremlinqConfiguration>(),
                    sectionName)));
        }
    }
}
