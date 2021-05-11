namespace ExRam.Gremlinq.Core
{
    public sealed class WhereTraversalStep : Step, IIsOptimizableInWhere
    {
        public WhereTraversalStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
