using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>inE()</c> step that maps vertices to their incoming incident edges.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
    public sealed class InEStep : DerivedLabelNamesStep
    {
        public static readonly InEStep NoLabels = new(ImmutableArray<string>.Empty);

        public InEStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
