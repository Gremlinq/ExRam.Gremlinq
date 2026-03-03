namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>tree()</c> step that collects traversal paths as tree structures.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#tree-step">Reference Documentation - Tree Step</seealso>
    public sealed class TreeStep : Step
    {
        /// <summary>Base class for <c>by()</c> modulators applied to the <c>tree()</c> step.</summary>
        public abstract class ByStep : Step
        {
            /// <inheritdoc />
            protected ByStep(SideEffectSemanticsChange sideEffectSemanticsChange = SideEffectSemanticsChange.None) : base(sideEffectSemanticsChange)
            {
            }
        }

        /// <summary>Represents a <c>by()</c> modulator using the identity projection on a <c>tree()</c> step.</summary>
        public sealed class ByIdentityStep : ByStep
        {
            /// <summary>Gets the singleton instance of <see cref="ByIdentityStep"/>.</summary>
            public static readonly ByIdentityStep Instance = new();

            private ByIdentityStep()
            {

            }
        }

        /// <summary>Represents a <c>by()</c> modulator with a key argument applied to a <c>tree()</c> step.</summary>
        public sealed class ByKeyStep : ByStep
        {
            /// <summary>Initializes a new instance of <see cref="ByKeyStep"/>.</summary>
            /// <param name="key">The property key.</param>
            public ByKeyStep(Key key)
            {
                Key = key;
            }

            /// <summary>Gets the property key.</summary>
            public Key Key { get; }
        }

        /// <summary>Gets the singleton instance of <see cref="TreeStep"/>.</summary>
        public static readonly TreeStep Instance = new();
    }
}
