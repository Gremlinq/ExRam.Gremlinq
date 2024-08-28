namespace ExRam.Gremlinq.Core.Steps
{
    public abstract class FilterStep : Step, IFilterStep
    {
        public sealed class ByTraversalStep : Step
        {
            public ByTraversalStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
            {
                Traversal = traversal;
            }

            public Traversal Traversal { get; }
        }

        protected FilterStep(SideEffectSemanticsChange sideEffectSemanticsChange) : base(sideEffectSemanticsChange)
        {

        }
    }
}
