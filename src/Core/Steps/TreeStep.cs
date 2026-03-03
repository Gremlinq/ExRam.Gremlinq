namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>tree()</c> step that collects traversal paths as tree structures.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#tree-step">Reference Documentation - Tree Step</seealso>
    public sealed class TreeStep : Step
    {
        /// <summary>Base class for <c>by()</c> modulators applied to the <c>tree()</c> step.</summary>
        public abstract class ByStep : Step
        {
            protected ByStep(SideEffectSemanticsChange sideEffectSemanticsChange = SideEffectSemanticsChange.None) : base(sideEffectSemanticsChange)
            {
            }
        }

        /// <summary>Represents a <c>by()</c> modulator using the identity projection on a <c>tree()</c> step.</summary>
        public sealed class ByIdentityStep : ByStep
        {
            public static readonly ByIdentityStep Instance = new();

            private ByIdentityStep()
            {

            }
        }

        /// <summary>Represents a <c>by()</c> modulator with a key argument applied to a <c>tree()</c> step.</summary>
        public sealed class ByKeyStep : ByStep
        {
            public ByKeyStep(Key key)
            {
                Key = key;
            }

            public Key Key { get; }
        }

        public static readonly TreeStep Instance = new();
    }
}
