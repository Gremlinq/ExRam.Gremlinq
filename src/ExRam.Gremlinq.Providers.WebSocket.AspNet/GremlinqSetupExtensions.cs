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

            public IConfigurableGremlinQuerySource Transform(IConfigurableGremlinQuerySource source)
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

        public static GremlinqSetup UseWebSocket(this GremlinqSetup setup)
        {
            return setup.RegisterTypes(serviceCollection => serviceCollection
                .AddSingleton<IGremlinQuerySourceTransformation, ConfigureLoggingGremlinQuerySourceTransformation>());
        }
    }
}
