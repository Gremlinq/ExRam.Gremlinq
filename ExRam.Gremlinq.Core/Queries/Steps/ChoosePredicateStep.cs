using ExRam.Gremlinq.Core.Serialization;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public sealed class ChoosePredicateStep : ChooseStep
    {
        public ChoosePredicateStep(P predicate, IGremlinQuery thenTraversal, Option<IGremlinQuery> elseTraversal = default) : base(thenTraversal, elseTraversal)
        {
            Predicate = predicate;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public P Predicate { get; }
    }
}