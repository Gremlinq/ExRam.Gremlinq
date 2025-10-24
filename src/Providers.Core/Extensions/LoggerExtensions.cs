using ExRam.Gremlinq.Core;

using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Providers.Core
{
    internal static partial class LoggerExtensions
    {
        [LoggerMessage(EventId = 0, Message = "Executing Gremlin query {requestId}.")]
        public static partial void LogQuery(this ILogger logger, LogLevel level, Guid requestId);

        [LoggerMessage(EventId = 0, Message = "Executing Gremlin query {requestId} with groovy script {script}.")]
        public static partial void LogQuery(this ILogger logger, LogLevel level, Guid requestId, string script);

        [LoggerMessage(EventId = 0, Message = "Executing Gremlin query {requestId} with groovy script {script} and parameter bindings {bindings}.")]
        public static partial void LogQuery(this ILogger logger, LogLevel level, Guid requestId, string script, Bindings bindings);
    }
}
