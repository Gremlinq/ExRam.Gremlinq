namespace ExRam.Gremlinq.Core.Steps
{
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
