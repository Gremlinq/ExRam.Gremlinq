using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>V()</c> step that reads vertices by their identifiers.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#graph-step">Reference Documentation - Graph Step</seealso>
    public sealed class VStep : Step
    {
        /// <summary>
        ///  Static instance of <see cref="VStep"/> representing a call to the V() operator without any ids passed. 
        /// </summary>
        public static readonly VStep NoIds = new (ImmutableArray<object>.Empty);

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

