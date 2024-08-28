namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class WhereTraversalStep : Step, IFilterStep
    {
        public WhereTraversalStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
