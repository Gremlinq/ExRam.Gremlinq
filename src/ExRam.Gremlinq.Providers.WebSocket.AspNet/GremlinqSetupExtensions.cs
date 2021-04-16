using System;
using ExRam.Gremlinq.Providers.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class ConfigureLoggingGremlinQuerySourceTransformation : IGremlinQuerySourceTransformation
        {
            private readonly IConfiguration _loggingSection;

            public ConfigureLoggingGremlinQuerySourceTransformation(IGremlinqConfiguration configuration)
            {
                _loggingSection = configuration
                    .GetSection("QueryLogging");
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                return source
                    .ConfigureEnvironment(environment => environment
                        .ConfigureOptions(options =>
                        {
                            if (Enum.TryParse<QueryLogVerbosity>(_loggingSection["Verbosity"], out var verbosity))
                                options = options.SetValue(WebSocketGremlinqOptions.QueryLogVerbosity, verbosity);

                            if (Enum.TryParse<LogLevel>(_loggingSection[$"{nameof(LogLevel)}"], out var logLevel))
                                options = options.SetValue(WebSocketGremlinqOptions.QueryLogLogLevel, logLevel);

                            if (Enum.TryParse<Formatting>(_loggingSection[$"{nameof(Formatting)}"], out var formatting))
                                options = options.SetValue(WebSocketGremlinqOptions.QueryLogFormatting, formatting);
                            
                            if (Enum.TryParse<GroovyFormatting>(_loggingSection[$"{nameof(GroovyFormatting)}"], out var groovyFormatting))
                                options = options.SetValue(WebSocketGremlinqOptions.QueryLogGroovyFormatting, groovyFormatting);

                            return options;
                        }));
            }
        }

        private sealed class UseProviderGremlinQuerySourceTransformation<TConfigurator> : IGremlinQuerySourceTransformation
            where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            private readonly string _sectionName;
            private readonly IGremlinqConfiguration _configuration;
            private readonly Func<TConfigurator, IConfiguration, TConfigurator> _configuratorTransformation;
            private readonly Func<IConfigurableGremlinQuerySource, Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> _providerChoice;
            
            public UseProviderGremlinQuerySourceTransformation(
                IGremlinqConfiguration configuration,
                string sectionName,
                Func<IConfigurableGremlinQuerySource, Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> providerChoice,
                Func<TConfigurator, IConfiguration, TConfigurator> configuratorTransformation)
            {
                _configuration = configuration;
                _sectionName = sectionName;
                _providerChoice = providerChoice;
                _configuratorTransformation = configuratorTransformation;
            }
            
            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                return _providerChoice(
                    source,
                    configurator =>
                    {
                        return _configuratorTransformation(configurator, _configuration.GetSection(_sectionName))
                            .ConfigureWebSocket(configurator =>
                            {
                                return configurator
                                    .Configure(_configuration);
                            });
                    });
            }
        }

        public static GremlinqSetup UseProvider<TConfigurator>(
            this GremlinqSetup setup,
            string sectionName,
            Func<IConfigurableGremlinQuerySource, Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> providerChoice,
            Func<TConfigurator, IConfiguration, TConfigurator> configuration) where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            return setup.RegisterTypes(serviceCollection => serviceCollection
                .AddSingleton<IGremlinQuerySourceTransformation, ConfigureLoggingGremlinQuerySourceTransformation>()
                .AddSingleton<IGremlinQuerySourceTransformation>(s => new UseProviderGremlinQuerySourceTransformation<TConfigurator>(
                    s.GetRequiredService<IGremlinqConfiguration>(),
                    sectionName,
                    providerChoice,
                    configuration)));
        }
    }
}
