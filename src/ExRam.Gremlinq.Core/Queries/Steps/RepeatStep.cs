namespace ExRam.Gremlinq.Core
{
    public sealed class RepeatStep : Step
    {
        public RepeatStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Traversal = traversal;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new RepeatStep(Traversal, semantics);

        public Traversal Traversal { get; }
    }
}
