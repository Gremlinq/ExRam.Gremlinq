using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class ByLambdaStep : Step
    {
        public Lambda Lambda { get; }

        public ByLambdaStep(Lambda lambda)
        {
            Lambda = lambda;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public sealed class ByMemberStep : Step
    {
        public ByMemberStep(object key, Order order)
        {
            Order = order;
            Key = key;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Order Order { get; }
        public object Key { get; }
    }

    public sealed class ByTraversalStep : Step
    {
        public Order Order { get; }
        public IGremlinQuery Traversal { get; }

        public ByTraversalStep(IGremlinQuery traversal, Order order)
        {
            Order = order;
            Traversal = traversal;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
