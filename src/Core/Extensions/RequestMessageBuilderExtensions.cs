using ExRam.Gremlinq.Core;

namespace Gremlin.Net.Driver.Messages
{
    internal static class RequestMessageBuilderExtensions
    {
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

