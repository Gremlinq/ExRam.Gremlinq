using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>bothE()</c> step that maps vertices to their incident edges in both directions.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
    public sealed class BothEStep : DerivedLabelNamesStep
    {
        /// <summary>Gets an instance without label restrictions.</summary>
        public static readonly BothEStep NoLabels = new(ImmutableArray<string>.Empty);

        /// <summary>Initializes a new instance of <see cref="BothEStep"/> with the specified edge labels.</summary>
        /// <param name="labels">The edge labels to filter by.</param>
        public BothEStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
