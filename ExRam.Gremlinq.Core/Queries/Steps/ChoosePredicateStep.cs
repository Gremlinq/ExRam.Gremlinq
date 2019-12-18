using Gremlin.Net.Process.Traversal;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public sealed class ChoosePredicateStep : ChooseStep
    {
        public ChoosePredicateStep(P predicate, IGremlinQueryBase thenTraversal, Option<IGremlinQueryBase> elseTraversal = default) : base(thenTraversal, elseTraversal)
        {
            Predicate = predicate;
        }

        public P Predicate { get; }
    }
}
