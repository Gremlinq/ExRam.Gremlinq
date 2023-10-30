using ExRam.Gremlinq.Core.Serialization;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public static class BytecodeExtensions
    {
        public static GroovyGremlinScript ToGroovyScript(this Bytecode bytecode, IGremlinQueryEnvironment environment, bool includeBindings = true) => GroovyWriter.ToGroovyScript(bytecode, environment, includeBindings);
    }
}
