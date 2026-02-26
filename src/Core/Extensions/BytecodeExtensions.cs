using ExRam.Gremlinq.Core.Serialization;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public static class BytecodeExtensions
    {
        public static GroovyGremlinScript ToGroovyScript(this Bytecode bytecode, IGremlinQueryEnvironment environment, bool includeBindings = true)
        {
            ArgumentNullException.ThrowIfNull(bytecode);
            ArgumentNullException.ThrowIfNull(environment);

            return GroovyWriter.ToGroovyScript(bytecode, environment, includeBindings);
        }
    }
}
