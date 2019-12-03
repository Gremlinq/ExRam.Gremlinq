using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class OrderStep : Step
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

        public sealed class ByTraversalStep : SingleTraversalArgumentStep
        {
            public Order Order { get; }

            public ByTraversalStep(IGremlinQuery traversal, Order order) : base(traversal)
            {
                Order = order;
            }
        }

        public static readonly OrderStep Instance = new OrderStep();
    }
}
