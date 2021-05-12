using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class ChoosePredicateStep : ChooseStep
    {
        public ChoosePredicateStep(P predicate, Traversal thenTraversal, Traversal? elseTraversal = default, QuerySemantics? semantics = default) : base(thenTraversal, elseTraversal, semantics)
        {
            Predicate = predicate;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new ChoosePredicateStep(Predicate, ThenTraversal, ElseTraversal, semantics);

        public P Predicate { get; }
    }
}
