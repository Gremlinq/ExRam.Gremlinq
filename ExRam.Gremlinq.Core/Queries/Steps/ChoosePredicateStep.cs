using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public sealed class ChoosePredicateStep : ChooseStep
    {
        public ChoosePredicateStep(P predicate, IGremlinQuery thenTraversal, Option<IGremlinQuery> elseTraversal = default) : base(thenTraversal, elseTraversal)
        {
            Predicate = predicate;
        }

        public P Predicate { get; }
    }
}
