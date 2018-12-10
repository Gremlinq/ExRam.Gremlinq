namespace ExRam.Gremlinq
{
    public sealed class SideEffectStep : SingleTraversalArgumentStep
    {
        public SideEffectStep(IGremlinQuery traversal) : base("sideEffect", traversal)
        {
        }
    }
}