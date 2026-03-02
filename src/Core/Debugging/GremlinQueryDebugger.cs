#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Provides pre-built <see cref="IGremlinQueryDebugger"/> instances.
    /// </summary>
    public static class GremlinQueryDebugger
    {
        /// <summary>
        /// A debugger that serializes queries to Groovy script representation.
        /// </summary>
        public static readonly IGremlinQueryDebugger Groovy = new GroovyGremlinQueryDebugger();
    }
}
