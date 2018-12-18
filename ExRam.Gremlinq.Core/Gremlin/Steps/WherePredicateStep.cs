using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class WherePredicateStep : Step
    {
        public WherePredicateStep(P predicate)
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
