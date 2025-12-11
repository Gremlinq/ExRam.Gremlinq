#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Provides debugging capabilities for Gremlin queries by converting bytecode to a readable format.
    /// </summary>
    public interface IGremlinQueryDebugger
    {
        /// <summary>
        /// Converts Gremlin bytecode to a human-readable debug representation.
        /// </summary>
        /// <param name="bytecode">The Gremlin bytecode to debug.</param>
        /// <param name="environment">The query environment providing context for debugging.</param>
        /// <returns>A string representation of the query for debugging purposes.</returns>
        string Debug(Bytecode bytecode, IGremlinQueryEnvironment environment);
    }
}
