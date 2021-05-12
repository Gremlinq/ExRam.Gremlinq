namespace ExRam.Gremlinq.Core
{
    public sealed class SideEffectStep : Step
    {
        public SideEffectStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Traversal = traversal;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new SideEffectStep(Traversal, semantics);

        public Traversal Traversal { get; }
    }
}
