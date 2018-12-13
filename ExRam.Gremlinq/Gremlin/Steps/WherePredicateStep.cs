using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
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