using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>order()</c> step that orders traversers.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#order-step">Reference Documentation - Order Step</seealso>
    public sealed class OrderStep : Step
    {
        /// <summary>Base class for <c>by()</c> modulators applied to the <c>order()</c> step.</summary>
        public abstract class ByStep : Step
        {
            protected ByStep(SideEffectSemanticsChange sideEffectSemanticsChange = SideEffectSemanticsChange.None) : base(sideEffectSemanticsChange)
            {
            }
        }

        /// <summary>Represents a <c>by()</c> modulator ordering by a member key and direction.</summary>
        public sealed class ByMemberStep : ByStep
        {
            public ByMemberStep(Key key, Order order)
            {
                ArgumentNullException.ThrowIfNull(order);

                Order = order;
                Key = key;
            }

            public Order Order { get; }
            public Key Key { get; }
        }

        /// <summary>Represents a <c>by()</c> modulator ordering by a traversal and direction.</summary>
        public sealed class ByTraversalStep : ByStep
        {
            public ByTraversalStep(Traversal traversal, Order order) : base(traversal.GetSideEffectSemanticsChange())
            {
                ArgumentNullException.ThrowIfNull(order);

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
            ArgumentNullException.ThrowIfNull(scope);

            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
