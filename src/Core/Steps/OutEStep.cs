using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>outE()</c> step that maps vertices to their outgoing incident edges.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
    public sealed class OutEStep : DerivedLabelNamesStep
    {
        public static readonly OutEStep NoLabels = new(ImmutableArray<string>.Empty);

        public OutEStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
