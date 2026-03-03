namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>inV()</c> step that maps an edge to its incoming (head) vertex.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
    public sealed class InVStep : Step
    {
        public static readonly InVStep Instance = new();
    }
}
