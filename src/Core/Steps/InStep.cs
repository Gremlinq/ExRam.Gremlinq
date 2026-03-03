using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>in()</c> step that maps vertices to their adjacent vertices via incoming edges.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
    public sealed class InStep : DerivedLabelNamesStep
    {
        public static readonly InStep NoLabels = new(ImmutableArray<string>.Empty);

        public InStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
