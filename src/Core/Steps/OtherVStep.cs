namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>otherV()</c> step that maps an edge to the vertex not traversed from.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
    public sealed class OtherVStep : Step
    {
        public static readonly OtherVStep Instance = new();
    }
}
