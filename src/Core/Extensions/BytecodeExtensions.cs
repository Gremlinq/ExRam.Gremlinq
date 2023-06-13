using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Serialization
{
    public static class BytecodeExtensions
    {
        public static GroovyGremlinQuery ToGroovy(this Bytecode bytecode, IGremlinQueryEnvironment environment, bool includeBindings = true) => GroovyWriter.ToGroovyGremlinQuery(bytecode, environment, includeBindings);
    }
}
