using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public readonly struct QueryLoggingOptions
    {
        public static readonly QueryLoggingOptions Default = new QueryLoggingOptions(LogLevel.Trace, QueryLoggingVerbosity.QueryOnly, Formatting.None);

        public QueryLoggingOptions(LogLevel logLevel, QueryLoggingVerbosity verbosity, Formatting formatting)
        {
            LogLevel = logLevel;
            Verbosity = verbosity;
            Formatting = formatting;
        }

        public QueryLoggingOptions SetLogLevel(LogLevel logLevel)
        {
            return new QueryLoggingOptions(logLevel, Verbosity, Formatting);
        }

        public QueryLoggingOptions SetQueryLoggingVerbosity(QueryLoggingVerbosity verbosity)
        {
            return new QueryLoggingOptions(LogLevel, verbosity, Formatting);
        }

        public QueryLoggingOptions SetFormatting(Formatting formatting)
        {
            return new QueryLoggingOptions(LogLevel, Verbosity, formatting);
        }

        public LogLevel LogLevel { get; }
        public Formatting Formatting { get; }
        public QueryLoggingVerbosity Verbosity { get; }
    }
}
