using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Base class for steps that take multiple sub-traversals as arguments.</summary>
    public abstract class MultiTraversalArgumentStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="MultiTraversalArgumentStep"/> with the specified sub-traversals.</summary>
        /// <param name="traversals">The sub-traversals.</param>
        protected MultiTraversalArgumentStep(ImmutableArray<Traversal> traversals) : base(traversals.GetSideEffectSemanticsChange())
        {
            Traversals = traversals;
        }

        /// <summary>Gets the sub-traversals.</summary>
        public ImmutableArray<Traversal> Traversals { get; }
    }
}
