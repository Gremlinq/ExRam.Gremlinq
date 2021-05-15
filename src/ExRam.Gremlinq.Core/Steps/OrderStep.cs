using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class OrderStep : Step
    {
        public abstract class ByStep : Step
        {
            protected ByStep() : base()
            {
            }
        }

        public sealed class ByLambdaStep : ByStep
        {
            public ByLambdaStep(ILambda lambda)
            {
                Lambda = lambda;
            }

            public ILambda Lambda { get; }
        }

        public sealed class ByMemberStep : ByStep
        {
            public ByMemberStep(Key key, Order order)
            {
                Order = order;
                Key = key;
            }

            public Order Order { get; }
            public Key Key { get; }
        }

        public sealed class ByTraversalStep : ByStep
        {
            public ByTraversalStep(Traversal traversal, Order order)
            {
                Traversal = traversal;
                Order = order;
            }

            public Order Order { get; }
            public Traversal Traversal { get; }
        }

        public static readonly OrderStep Global = new(Scope.Global);
        public static readonly OrderStep Local = new(Scope.Local);

        public OrderStep(Scope scope) : base()
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
