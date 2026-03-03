namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>where(traversal)</c> step that filters by a traversal-based predicate.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#where-step">Reference Documentation - Where Step</seealso>
    public sealed class WhereTraversalStep : Step, IFilterStep
    {
        /// <summary>Initializes a new instance of <see cref="WhereTraversalStep"/> with the specified traversal.</summary>
        /// <param name="traversal">The traversal-based filter predicate.</param>
        public WhereTraversalStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        /// <summary>Gets the filter traversal.</summary>
        public Traversal Traversal { get; }
    }
}
