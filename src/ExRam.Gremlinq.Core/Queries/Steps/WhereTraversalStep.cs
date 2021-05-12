namespace ExRam.Gremlinq.Core
{
    public sealed class WhereTraversalStep : Step, IIsOptimizableInWhere
    {
        public WhereTraversalStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Traversal = traversal;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new WhereTraversalStep(Traversal, semantics);

        public Traversal Traversal { get; }
    }
}
