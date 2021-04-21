// ReSharper disable HeapView.PossibleBoxingAllocation
using System;
using System.Collections.Generic;
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
        private sealed class ConfigureWebSocketConfiguratorTransformation : IWebSocketConfiguratorTransformation
        {
            private readonly Func<IWebSocketConfigurator, IWebSocketConfigurator> _transformation;

            public ConfigureWebSocketConfiguratorTransformation(Func<IWebSocketConfigurator, IWebSocketConfigurator> transformation)
            {
                _transformation = transformation;
            }

            public IWebSocketConfigurator Transform(IWebSocketConfigurator configurator) => _transformation(configurator);
        }

        private sealed class UseProviderGremlinQuerySourceTransformation<TProviderConfigurator> : IGremlinQuerySourceTransformation
            where TProviderConfigurator : IProviderConfigurator<TProviderConfigurator>
        {
            private readonly string _sectionName;
            private readonly IGremlinqConfiguration _generalSection;
            private readonly IEnumerable<IWebSocketConfiguratorTransformation> _webSocketConfiguratorTransformations;
            private readonly Func<TProviderConfigurator, IConfiguration, TProviderConfigurator>? _extraConfiguratorTransformation;
            private readonly Func<TProviderConfigurator, IConfiguration, TProviderConfigurator> _providerConfiguratorTransformation;
            private readonly Func<IConfigurableGremlinQuerySource, Func<TProviderConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> _providerChoice;
            
            public UseProviderGremlinQuerySourceTransformation(
                IGremlinqConfiguration generalSection,
                string sectionName,
                Func<IConfigurableGremlinQuerySource, Func<TProviderConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> providerChoice,
                Func<TProviderConfigurator, IConfiguration, TProviderConfigurator> providerConfiguratorTransformation,
                Func<TProviderConfigurator, IConfiguration, TProviderConfigurator>? extraConfiguratorTransformation,
                IEnumerable<IWebSocketConfiguratorTransformation> webSocketConfiguratorTransformations)
            {
                _sectionName = sectionName;
                _generalSection = generalSection;
                _providerChoice = providerChoice;
                _extraConfiguratorTransformation = extraConfiguratorTransformation;
                _providerConfiguratorTransformation = providerConfiguratorTransformation;
                _webSocketConfiguratorTransformations = webSocketConfiguratorTransformations;
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

                        if (configurator is IWebSocketProviderConfigurator<TProviderConfigurator> webSocketProviderConfigurator2)
                        {
                            configurator = webSocketProviderConfigurator2
                                .ConfigureWebSocket(webSocketConfigurator =>
                                {
                                    foreach (var webSocketConfiguratorTransformation in _webSocketConfiguratorTransformations)
                                    {
                                        webSocketConfigurator = webSocketConfiguratorTransformation
                                            .Transform(webSocketConfigurator);
                                    }

                                    return webSocketConfigurator;
                                });
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
                    extraConfiguration,
                    s.GetRequiredService<IEnumerable<IWebSocketConfiguratorTransformation>>())));
        }

        public static GremlinqSetup ConfigureWebSocket(this GremlinqSetup setup, Func<IWebSocketConfigurator, IWebSocketConfigurator> transformation)
        {
            return setup.RegisterTypes(serviceCollection => serviceCollection
                .AddSingleton<IWebSocketConfiguratorTransformation>(new ConfigureWebSocketConfiguratorTransformation(transformation)));
        }
    }
}
