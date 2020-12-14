using ExRam.Gremlinq.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public static class WebSocketGremlinqOptions
    {
        public static GremlinqOption<LogLevel> QueryLogLogLevel = new GremlinqOption<LogLevel>(LogLevel.Debug);
        public static GremlinqOption<Formatting> QueryLogFormatting = new GremlinqOption<Formatting>(Formatting.None);
        public static GremlinqOption<QueryLogVerbosity> QueryLogVerbosity = new GremlinqOption<QueryLogVerbosity>(Core.QueryLogVerbosity.QueryOnly);
        public static GremlinqOption<GroovyFormatting> QueryLogGroovyFormatting = new GremlinqOption<GroovyFormatting>(GroovyFormatting.BindingsOnly);
    }
}