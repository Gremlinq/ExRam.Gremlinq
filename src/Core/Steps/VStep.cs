using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>V()</c> step that reads vertices by their identifiers.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#graph-step">Reference Documentation - Graph Step</seealso>
    public sealed class VStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="VStep"/> with the specified vertex identifiers.</summary>
        /// <param name="ids">The vertex identifiers to read.</param>
        public VStep(ImmutableArray<object> ids)
        {
            Ids = ids;
        }

        /// <summary>Gets the vertex identifiers.</summary>
        public ImmutableArray<object> Ids { get; }
    }
}

