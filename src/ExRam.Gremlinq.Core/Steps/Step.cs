namespace ExRam.Gremlinq.Core.Steps
{
    public abstract class Step
    {
        protected Step(SideEffectSemanticsChange sideEffectSemanticsChange = SideEffectSemanticsChange.None)
        {
            SideEffectSemanticsChange = sideEffectSemanticsChange;
        }

        public SideEffectSemanticsChange SideEffectSemanticsChange { get; }
    }
}
