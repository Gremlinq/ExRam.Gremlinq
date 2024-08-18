using System.Collections.Immutable;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Serialization;

using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.Core
{
    internal static class RequestMessageExtensions
    {
        public static GroovyGremlinScript? TryGetGroovyScript(this RequestMessage requestMessage, IGremlinQueryEnvironment environment, bool includeBindings)
        {
            if (requestMessage.Operation == Tokens.OpsBytecode)
            {
                if (requestMessage.Arguments.TryGetValue(Tokens.ArgsGremlin, out var bytecodeObject) && bytecodeObject is Bytecode bytecode)
                    return bytecode.ToGroovyScript(environment, includeBindings);
            }
            else if (requestMessage.Operation == Tokens.OpsEval)
            {
                if (requestMessage.Arguments.TryGetValue(Tokens.ArgsGremlin, out var scriptObject) && scriptObject is string script)
                {
                    return GroovyGremlinScript.From(
                        script,
                        includeBindings && requestMessage.Arguments.TryGetValue(Tokens.ArgsBindings, out var bindingsObject) && bindingsObject is IReadOnlyDictionary<string, object?> bindings
                            ? bindings.ToImmutableDictionary()
                            : null);
                }
            }

            return default;
        }

        public static RequestMessage.Builder Rebuild(this RequestMessage message) => RequestMessage
            .Build(message.Operation)
            .OverrideRequestId(message.RequestId)
            .Processor(message.Processor)
            .AddArguments(message.Arguments);
    }
}
