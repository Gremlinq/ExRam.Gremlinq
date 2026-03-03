namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>bothV()</c> step that maps an edge to both its incident vertices.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
    public sealed class BothVStep : Step
    {
        /// <summary>Gets the singleton instance of <see cref="BothVStep"/>.</summary>
        public static readonly BothVStep Instance = new();
    }
}
