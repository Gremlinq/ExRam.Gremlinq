using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class ChoosePredicateStep : ChooseStep
    {
        public ChoosePredicateStep(P predicate, Traversal thenTraversal, Traversal? elseTraversal = default) : base(thenTraversal, elseTraversal, thenTraversal.GetSideEffectSemanticsChange() | elseTraversal.GetSideEffectSemanticsChange())
        {
            Predicate = predicate;
        }

        public P Predicate { get; }
    }
}
