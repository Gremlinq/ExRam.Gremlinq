namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>group()</c> step that organizes elements into a dictionary.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#group-step">Reference Documentation - Group Step</seealso>
    public sealed class GroupStep : Step
    {
        /// <summary>Base class for <c>by()</c> modulators applied to the <c>group()</c> step.</summary>
        public abstract class ByStep : Step
        {
            protected ByStep(SideEffectSemanticsChange sideEffectSemanticsChange = SideEffectSemanticsChange.None) : base(sideEffectSemanticsChange)
            {
            }
        }

        /// <summary>Represents a <c>by()</c> modulator with a traversal argument applied to a <c>group()</c> step.</summary>
        public sealed class ByTraversalStep : ByStep
        {
            public ByTraversalStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
            {
                Traversal = traversal;
            }

            public Traversal Traversal { get; }
        }

        /// <summary>Represents a <c>by()</c> modulator with a key argument applied to a <c>group()</c> step.</summary>
        public sealed class ByKeyStep : ByStep
        {
            public ByKeyStep(Key key)
            {
                Key = key;
            }

            public Key Key { get; }
        }

        public static readonly GroupStep Instance = new();
    }
}
