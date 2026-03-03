using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>out()</c> step that maps vertices to their adjacent vertices via outgoing edges.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
    public sealed class OutStep : DerivedLabelNamesStep
    {
        public static readonly OutStep NoLabels = new(ImmutableArray<string>.Empty);

        public OutStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
