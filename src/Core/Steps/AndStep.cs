using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>and()</c> step that filters traversers by requiring all sub-traversals to yield a result.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#and-step">Reference Documentation - And Step</seealso>
    public sealed class AndStep : LogicalStep<AndStep>, IFilterStep
    {
        /// <summary>Initializes a new instance of <see cref="AndStep"/> with the given sub-traversals.</summary>
        /// <param name="traversals">The sub-traversals that must all yield results.</param>
        public AndStep(ImmutableArray<Traversal> traversals) : base(traversals)
        {
        }
    }
}
