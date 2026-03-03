using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>inE()</c> step that maps vertices to their incoming incident edges.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
    public sealed class InEStep : DerivedLabelNamesStep
    {
        /// <summary>Gets an instance without label restrictions.</summary>
        public static readonly InEStep NoLabels = new(ImmutableArray<string>.Empty);

        /// <summary>Initializes a new instance of <see cref="InEStep"/> with the specified edge labels.</summary>
        /// <param name="labels">The edge labels to filter by.</param>
        public InEStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
