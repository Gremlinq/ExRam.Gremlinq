using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>E()</c> step that reads edges by their identifiers.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#graph-step">Reference Documentation - Graph Step</seealso>
    public sealed class EStep : Step
    {
        /// <summary>
        ///  Static instance of <see cref="EStep"/> representing a call to the E() operator without any ids passed. 
        /// </summary>
        public static readonly EStep Empty = new (ImmutableArray<object>.Empty);

        /// <summary>Initializes a new instance of <see cref="EStep"/> with the specified edge identifiers.</summary>
        /// <param name="ids">The edge identifiers to read.</param>
        public EStep(ImmutableArray<object> ids)
        {
            Ids = ids;
        }

        /// <summary>Gets the edge identifiers.</summary>
        public ImmutableArray<object> Ids { get; }
    }
}

