namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>tree()</c> step that collects traversal paths as tree structures.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#tree-step">Reference Documentation - Tree Step</seealso>
    public sealed class TreeStep : Step
    {
        public abstract class ByStep : Step
        {
            protected ByStep(SideEffectSemanticsChange sideEffectSemanticsChange = SideEffectSemanticsChange.None) : base(sideEffectSemanticsChange)
            {
            }
        }

        public sealed class ByIdentityStep : ByStep
        {
            public static readonly ByIdentityStep Instance = new();

            private ByIdentityStep()
            {

            }
        }

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
