using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class OrderStep : Step
    {
        public abstract class ByStep : Step
        {
            protected ByStep(SideEffectSemanticsChange sideEffectSemanticsChange = SideEffectSemanticsChange.None) : base(sideEffectSemanticsChange)
            {
            }
        }

        public sealed class ByLambdaStep : ByStep
        {
            public ByLambdaStep(ILambda lambda) : base(SideEffectSemanticsChange.Write)
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
            public ByTraversalStep(Traversal traversal, Order order) : base(traversal.GetSideEffectSemanticsChange())
            {
                Traversal = traversal;
                Order = order;
            }

            public Order Order { get; }
            public Traversal Traversal { get; }
        }

        public static readonly OrderStep Global = new(Scope.Global);
        public static readonly OrderStep Local = new(Scope.Local);

        public OrderStep(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
