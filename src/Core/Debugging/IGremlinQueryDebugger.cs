#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Provides debug output for Gremlin queries by converting bytecode into a human-readable string.
    /// </summary>
    public interface IGremlinQueryDebugger
    {
        /// <summary>
        /// Converts the specified bytecode into a human-readable debug string.
        /// </summary>
        /// <param name="bytecode">The Gremlin bytecode to debug.</param>
        /// <param name="environment">The query environment.</param>
        /// <returns>A debug representation of the query.</returns>
        string Debug(Bytecode bytecode, IGremlinQueryEnvironment environment);
    }
}
