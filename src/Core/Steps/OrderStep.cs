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
            /// <inheritdoc />
            protected ByStep(SideEffectSemanticsChange sideEffectSemanticsChange = SideEffectSemanticsChange.None) : base(sideEffectSemanticsChange)
            {
            }
        }

        /// <summary>Represents a <c>by()</c> modulator ordering by a member key and direction.</summary>
        public sealed class ByMemberStep : ByStep
        {
            /// <summary>Initializes a new instance of <see cref="ByMemberStep"/>.</summary>
            /// <param name="key">The member key to order by.</param>
            /// <param name="order">The sort direction.</param>
            public ByMemberStep(Key key, Order order)
            {
                ArgumentNullException.ThrowIfNull(order);

                Order = order;
                Key = key;
            }

            /// <summary>Gets the sort direction.</summary>
            public Order Order { get; }
            /// <summary>Gets the member key.</summary>
            public Key Key { get; }
        }

        /// <summary>Represents a <c>by()</c> modulator ordering by a traversal and direction.</summary>
        public sealed class ByTraversalStep : ByStep
        {
            /// <summary>Initializes a new instance of <see cref="ByTraversalStep"/>.</summary>
            /// <param name="traversal">The traversal to order by.</param>
            /// <param name="order">The sort direction.</param>
            public ByTraversalStep(Traversal traversal, Order order) : base(traversal.GetSideEffectSemanticsChange())
            {
                ArgumentNullException.ThrowIfNull(order);

                Traversal = traversal;
                Order = order;
            }

            /// <summary>Gets the sort direction.</summary>
            public Order Order { get; }
            /// <summary>Gets the ordering traversal.</summary>
            public Traversal Traversal { get; }
        }

        /// <summary>Gets the global-scoped instance.</summary>
        public static readonly OrderStep Global = new(Scope.Global);
        /// <summary>Gets the local-scoped instance.</summary>
        public static readonly OrderStep Local = new(Scope.Local);

        /// <summary>Initializes a new instance of <see cref="OrderStep"/> with the specified scope.</summary>
        /// <param name="scope">The scope of the order operation.</param>
        public OrderStep(Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            Scope = scope;
        }

        /// <summary>Gets the scope.</summary>
        public Scope Scope { get; }
    }
}
