namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>outV()</c> step that maps an edge to its outgoing (tail) vertex.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
    public sealed class OutVStep : Step
    {
        /// <summary>Gets the singleton instance of <see cref="OutVStep"/>.</summary>
        public static readonly OutVStep Instance = new();
    }
}
