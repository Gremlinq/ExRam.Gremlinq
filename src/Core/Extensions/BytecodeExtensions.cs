using ExRam.Gremlinq.Core.Serialization;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    /// <summary>Extension methods for <see cref="Bytecode"/>.</summary>
    public static class BytecodeExtensions
    {
        /// <summary>
        /// Converts the Gremlin bytecode to a Groovy script representation.
        /// </summary>
        /// <param name="bytecode">The bytecode to convert.</param>
        /// <param name="environment">The query environment.</param>
        /// <param name="includeBindings">Whether to include bindings in the output.</param>
        public static GroovyGremlinScript ToGroovyScript(this Bytecode bytecode, IGremlinQueryEnvironment environment, bool includeBindings = true)
        {
            ArgumentNullException.ThrowIfNull(bytecode);
            ArgumentNullException.ThrowIfNull(environment);

            return GroovyWriter.ToGroovyScript(bytecode, environment, includeBindings);
        }
    }
}
