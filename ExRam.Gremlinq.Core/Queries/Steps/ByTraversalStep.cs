using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class ByLambdaStep : Step
    {
        public ILambda Lambda { get; }

        public ByLambdaStep(ILambda lambda)
        {
            Lambda = lambda;
        }
    }

    public sealed class ByMemberStep : Step
    {
        public ByMemberStep(object key, Order order)
        {
            Order = order;
            Key = key;
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
    }
}
