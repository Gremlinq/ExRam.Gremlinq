using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents a Gremlin <c>choose()</c> step with a predicate-based condition.</summary>
    public sealed class ChoosePredicateStep : ChooseStep
    {
        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
        /// <summary>Initializes a new instance of <see cref="ChoosePredicateStep"/>.</summary>
        /// <param name="predicate">The predicate condition.</param>
        /// <param name="thenTraversal">The traversal to execute when the predicate is true.</param>
        /// <param name="elseTraversal">The optional traversal to execute when the predicate is false.</param>
        public ChoosePredicateStep(P predicate, Traversal thenTraversal, Traversal? elseTraversal = null) : base(thenTraversal, elseTraversal, thenTraversal.GetSideEffectSemanticsChange() | (elseTraversal?.GetSideEffectSemanticsChange() ?? SideEffectSemanticsChange.None))
        {
            ArgumentNullException.ThrowIfNull(predicate);

            Predicate = predicate;
        }

        /// <summary>Gets the predicate condition.</summary>
        public P Predicate { get; }
    }
}
