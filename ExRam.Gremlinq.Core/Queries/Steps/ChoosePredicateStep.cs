using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class ChoosePredicateStep : ChooseStep
    {
        public ChoosePredicateStep(P predicate, IGremlinQueryBase thenTraversal, IGremlinQueryBase? elseTraversal = default) : base(thenTraversal, elseTraversal)
        {
            Predicate = predicate;
        }

        public P Predicate { get; }
    }
}
