using System.Reflection;
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
        public ByMemberStep(MemberInfo member, Order order)
        {
            Order = order;
            Member = member;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Order Order { get; }
        public MemberInfo Member { get; }
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
