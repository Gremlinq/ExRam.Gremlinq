using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>or()</c> step that filters traversers by requiring at least one sub-traversal to yield a result.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#or-step">Reference Documentation - Or Step</seealso>
    public sealed class OrStep : LogicalStep<OrStep>, IFilterStep
    {
        /// <summary>Initializes a new instance of <see cref="OrStep"/> with the given sub-traversals.</summary>
        /// <param name="traversals">The sub-traversals of which at least one must yield results.</param>
        public OrStep(ImmutableArray<Traversal> traversals) : base(traversals)
        {
        }
    }
}
