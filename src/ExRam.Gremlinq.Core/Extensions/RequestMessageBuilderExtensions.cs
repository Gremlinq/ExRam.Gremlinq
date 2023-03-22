using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Serialization;
using Microsoft.Extensions.Logging;

namespace Gremlin.Net.Driver.Messages
{
    internal static class RequestMessageBuilderExtensions
    {
        public static RequestMessage.Builder OverrideRequestId(this RequestMessage.Builder builder, BytecodeGremlinQuery query, IGremlinQueryEnvironment environment)
        {
            if (!Guid.TryParse(query.Id, out var requestId))
            {
                requestId = Guid.NewGuid();
                environment.Logger.LogInformation($"Mapping query id {query.Id} to request id {requestId}.");
            }

            return builder
                .OverrideRequestId(requestId);
        }

        public static RequestMessage.Builder AddAlias(this RequestMessage.Builder builder, IGremlinQueryEnvironment environment)
        {
            return builder
                .AddArgument(
                    Tokens.ArgsAliases,
                    new Dictionary<string, string>
                    {
                        { "g", environment.Options.GetValue(GremlinqOption.Alias) }
                    });
        }
    }
}

