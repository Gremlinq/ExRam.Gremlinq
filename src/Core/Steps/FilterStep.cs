namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Base class for filter steps that reduce the traversal stream.</summary>
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
