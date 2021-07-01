namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class HasTraversalStep : Step, IIsOptimizableInWhere
    { 
        public HasTraversalStep(Key key, Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Key = key;
            Traversal = traversal;
        }

        public Key Key { get; }
        public Traversal Traversal { get; }
    }
}
