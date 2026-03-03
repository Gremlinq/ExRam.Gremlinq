namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Base class for all Gremlin traversal steps.</summary>
    public abstract class Step
    {
        protected Step(SideEffectSemanticsChange sideEffectSemanticsChange = SideEffectSemanticsChange.None)
        {
            SideEffectSemanticsChange = sideEffectSemanticsChange;
        }

        public SideEffectSemanticsChange SideEffectSemanticsChange { get; }
    }
}
