namespace ExRam.Gremlinq.Core
{
    public sealed class HasTraversalStep : Step, IIsOptimizableInWhere
    { 
        public HasTraversalStep(Key key, Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Key = key;
            Traversal = traversal;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new HasTraversalStep(Key, Traversal, semantics);

        public Key Key { get; }
        public Traversal Traversal { get; }
    }
}
