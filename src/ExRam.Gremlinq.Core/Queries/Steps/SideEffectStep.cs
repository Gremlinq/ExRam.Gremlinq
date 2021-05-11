namespace ExRam.Gremlinq.Core
{
    public sealed class SideEffectStep : Step
    {
        public SideEffectStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
