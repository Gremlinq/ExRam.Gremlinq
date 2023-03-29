using ExRam.Gremlinq.Core.Serialization;

using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Process.Traversal;

namespace Gremlin.Net.Driver
{
    internal static class RequestMessageExtensions
    {
        public static GroovyGremlinQuery? TryGetGroovyQuery(this RequestMessage requestMessage)
        {
            if (requestMessage.Operation == Tokens.OpsBytecode)
            {
                if (requestMessage.Arguments.TryGetValue(Tokens.ArgsGremlin, out var bytecodeObject) && bytecodeObject is Bytecode bytecode)
                    return bytecode.ToGroovy();
            }
            else if (requestMessage.Operation == Tokens.OpsEval)
            {
                if (requestMessage.Arguments.TryGetValue(Tokens.ArgsGremlin, out var scriptObject) && scriptObject is string script)
                {
                    if (requestMessage.Arguments.TryGetValue(Tokens.ArgsBindings, out var bindingsObject) && bindingsObject is IReadOnlyDictionary<string, object> bindings)
                        return new GroovyGremlinQuery(script, bindings);
                }
            }

            return default;
        }
    }
}
