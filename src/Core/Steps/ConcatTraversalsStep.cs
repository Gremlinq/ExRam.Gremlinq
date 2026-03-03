using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>concat()</c> step with traversal arguments.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#concat-step">Reference Documentation - Concat Step</seealso>
    public sealed class ConcatTraversalsStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="ConcatTraversalsStep"/> with the specified traversals.</summary>
        /// <param name="traversals">The traversals whose string results are concatenated.</param>
        public ConcatTraversalsStep(ImmutableArray<Traversal> traversals)
        {
            Traversals = traversals;
        }

        /// <summary>Gets the traversals whose string results are concatenated.</summary>
        public ImmutableArray<Traversal> Traversals { get; }
    }
}
