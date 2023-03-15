using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    internal sealed class GroovyGremlinQueryDebugger : IGremlinQueryDebugger
    {
        public string Debug(BytecodeGremlinQuery serializedQuery, IGremlinQueryEnvironment environment) => new GroovyWriter()
            .Append(serializedQuery.Bytecode)
            .ToString();
    }
}
