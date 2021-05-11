using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class OrderStep : Step
    {
        public abstract class ByStep : Step
        {
            protected ByStep(QuerySemantics? semantics = default) : base(semantics)
            {
            }
        }

        public sealed class ByLambdaStep : ByStep
        {
            public ILambda Lambda { get; }

            public ByLambdaStep(ILambda lambda, QuerySemantics? semantics = default) : base(semantics)
            {
                Lambda = lambda;
            }
        }

        public sealed class ByMemberStep : ByStep
        {
            public ByMemberStep(Key key, Order order, QuerySemantics? semantics = default) : base(semantics)
            {
                Order = order;
                Key = key;
            }

            public Order Order { get; }
            public Key Key { get; }
        }

        public sealed class ByTraversalStep : ByStep
        {
            public ByTraversalStep(Traversal traversal, Order order, QuerySemantics? semantics = default) : base(semantics)
            {
                Traversal = traversal;
                Order = order;
            }

            public Order Order { get; }
            public Traversal Traversal { get; }
        }

        public static readonly OrderStep Global = new(Scope.Global);
        public static readonly OrderStep Local = new(Scope.Local);

        public OrderStep(Scope scope, QuerySemantics? semantics = default) : base(semantics)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
