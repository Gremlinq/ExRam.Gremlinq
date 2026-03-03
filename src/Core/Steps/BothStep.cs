using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>both()</c> step that maps vertices to their adjacent vertices via both directions.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
    public sealed class BothStep : DerivedLabelNamesStep
    {
        /// <summary>Gets an instance without label restrictions.</summary>
        public static readonly BothStep NoLabels = new(ImmutableArray<string>.Empty);

        /// <summary>Initializes a new instance of <see cref="BothStep"/> with the specified edge labels.</summary>
        /// <param name="labels">The edge labels to traverse.</param>
        public BothStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
