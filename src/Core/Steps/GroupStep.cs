namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>group()</c> step that organizes elements into a dictionary.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#group-step">Reference Documentation - Group Step</seealso>
    public sealed class GroupStep : Step
    {
        /// <summary>Base class for <c>by()</c> modulators applied to the <c>group()</c> step.</summary>
        public abstract class ByStep : Step
        {
            /// <inheritdoc />
            protected ByStep(SideEffectSemanticsChange sideEffectSemanticsChange = SideEffectSemanticsChange.None) : base(sideEffectSemanticsChange)
            {
            }
        }

        /// <summary>Represents a <c>by()</c> modulator with a traversal argument applied to a <c>group()</c> step.</summary>
        public sealed class ByTraversalStep : ByStep
        {
            /// <summary>Initializes a new instance of <see cref="ByTraversalStep"/>.</summary>
            /// <param name="traversal">The by-modulator traversal.</param>
            public ByTraversalStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
            {
                Traversal = traversal;
            }

            /// <summary>Gets the by-modulator traversal.</summary>
            public Traversal Traversal { get; }
        }

        /// <summary>Represents a <c>by()</c> modulator with a key argument applied to a <c>group()</c> step.</summary>
        public sealed class ByKeyStep : ByStep
        {
            /// <summary>Initializes a new instance of <see cref="ByKeyStep"/>.</summary>
            /// <param name="key">The property key to group by.</param>
            public ByKeyStep(Key key)
            {
                Key = key;
            }

            /// <summary>Gets the property key.</summary>
            public Key Key { get; }
        }

        /// <summary>Gets the singleton instance of <see cref="GroupStep"/>.</summary>
        public static readonly GroupStep Instance = new();
    }
}
