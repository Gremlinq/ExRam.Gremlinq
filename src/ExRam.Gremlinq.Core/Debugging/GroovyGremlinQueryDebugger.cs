using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal sealed class GroovyGremlinQueryDebugger : IGremlinQueryDebugger
    {
        public string Debug(Bytecode bytecode, IGremlinQueryEnvironment environment) => new GroovyWriter()
            .Append(bytecode)
            .ToString();
    }
}
