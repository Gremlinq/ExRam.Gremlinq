using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core.Tests.Extensions
{
    internal static class RequestMessageExtensions
    {
        public static object? SessionId(this RequestMessage request) => request.Arguments.TryGetValue(Tokens.ArgsSession, out var sessionId)
            ? sessionId
            : null;
    }
}
