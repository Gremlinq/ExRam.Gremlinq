using Gremlin.Net.Driver.Messages;

namespace Gremlin.Net.Driver
{
    internal static class RequestMessageBuilderExtensions
    {
        public static RequestMessage.Builder AddArguments(this RequestMessage.Builder builder, IEnumerable<KeyValuePair<string, object>> arguments)
        {
            foreach (var argument in arguments)
            {
                builder = builder
                    .AddArgument(argument.Key, argument.Value);
            }

            return builder;
        }
    }
}
