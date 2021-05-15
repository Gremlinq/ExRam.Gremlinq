// ReSharper disable HeapView.PossibleBoxingAllocation
using System;

using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.WebSocket;
using Microsoft.Extensions.Configuration;
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
            private readonly string _sectionName;
            private readonly IGremlinqConfiguration _generalSection;
            private readonly Func<TProviderConfigurator, IConfiguration, TProviderConfigurator>? _extraConfiguratorTransformation;
            private readonly Func<TProviderConfigurator, IConfiguration, TProviderConfigurator> _providerConfiguratorTransformation;
            private readonly Func<IConfigurableGremlinQuerySource, Func<TProviderConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> _providerChoice;
            
            public UseProviderGremlinQuerySourceTransformation(
                IGremlinqConfiguration generalSection,
                string sectionName,
                Func<IConfigurableGremlinQuerySource, Func<TProviderConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> providerChoice,
                Func<TProviderConfigurator, IConfiguration, TProviderConfigurator> providerConfiguratorTransformation,
                Func<TProviderConfigurator, IConfiguration, TProviderConfigurator>? extraConfiguratorTransformation)
            {
                _sectionName = sectionName;
                _generalSection = generalSection;
                _providerChoice = providerChoice;
                _extraConfiguratorTransformation = extraConfiguratorTransformation;
                _providerConfiguratorTransformation = providerConfiguratorTransformation;
            }
            
            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                var providerSection = _generalSection
                    .GetSection(_sectionName);

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
                        if (configurator is IWebSocketProviderConfigurator<TProviderConfigurator> webSocketProviderConfigurator1)
                        {
                            configurator = webSocketProviderConfigurator1
                                .ConfigureWebSocket(webSocketConfigurator => webSocketConfigurator
                                    .ConfigureFrom(_generalSection)
                                    .ConfigureFrom(providerSection));
                        }

                        configurator = _providerConfiguratorTransformation(
                            configurator,
                            providerSection);

                        if (_extraConfiguratorTransformation is { } extraConfiguratorTransformation)
                        {
                            configurator = extraConfiguratorTransformation(
                                configurator,
                                providerSection);
                        }

                        return configurator;
                    });
            }
        }

        public static GremlinqSetup UseProvider<TConfigurator>(
            this GremlinqSetup setup,
            string sectionName,
            Func<IConfigurableGremlinQuerySource, Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> providerChoice,
            Func<TConfigurator, IConfiguration, TConfigurator> configuration,
            Func<TConfigurator, IConfiguration, TConfigurator>? extraConfiguration = null) where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            return setup.RegisterTypes(serviceCollection => serviceCollection
                .AddSingleton<IGremlinQuerySourceTransformation>(s => new UseProviderGremlinQuerySourceTransformation<TConfigurator>(
                    s.GetRequiredService<IGremlinqConfiguration>(),
                    sectionName,
                    providerChoice,
                    configuration,
                    extraConfiguration)));
        }
    }
}
