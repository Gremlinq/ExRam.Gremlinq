namespace ExRam.Gremlinq
{
    public sealed class WherePredicateStep : Step
    {
        public WherePredicateStep(P predicate)
        {
            Predicate = predicate;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public P Predicate { get; }
    }

    public sealed class WhereTraversalStep : SingleTraversalArgumentStep
    {
        public WhereTraversalStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
