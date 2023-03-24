using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Serialization;
using Microsoft.Extensions.Logging;

namespace Gremlin.Net.Driver.Messages
{
    internal static class RequestMessageBuilderExtensions
    {
        public static RequestMessage.Builder AddAlias(this RequestMessage.Builder builder, IGremlinQueryEnvironment environment)
        {
            return environment.Options.GetValue(GremlinqOption.Alias) is { } alias && alias != "g"
                ? builder
                    .AddArgument(
                        Tokens.ArgsAliases,
                        new Dictionary<string, string>
                        {
                            { "g", alias }
                        })
                : builder;
        }
    }
}

