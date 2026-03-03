using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents a Gremlin <c>choose()</c> step with a predicate-based condition.</summary>
    public sealed class ChoosePredicateStep : ChooseStep
    {
        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
        public ChoosePredicateStep(P predicate, Traversal thenTraversal, Traversal? elseTraversal = null) : base(thenTraversal, elseTraversal, thenTraversal.GetSideEffectSemanticsChange() | elseTraversal.GetSideEffectSemanticsChange())
        {
            ArgumentNullException.ThrowIfNull(predicate);

            Predicate = predicate;
        }

        public P Predicate { get; }
    }
}
