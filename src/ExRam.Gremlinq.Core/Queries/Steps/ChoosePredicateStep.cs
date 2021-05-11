using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class ChoosePredicateStep : ChooseStep
    {
        public ChoosePredicateStep(P predicate, Traversal thenTraversal, Traversal? elseTraversal = default, QuerySemantics? semantics = default) : base(thenTraversal, elseTraversal, semantics)
        {
            Predicate = predicate;
        }

        public P Predicate { get; }
    }
}
