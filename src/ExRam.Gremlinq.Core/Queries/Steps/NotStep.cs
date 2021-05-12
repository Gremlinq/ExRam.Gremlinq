namespace ExRam.Gremlinq.Core
{
    public sealed class NotStep : Step, IIsOptimizableInWhere
    {
        public NotStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Traversal = traversal;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new NotStep(Traversal, semantics);

        public Traversal Traversal { get; }
    }
}
