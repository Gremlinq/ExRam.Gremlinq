namespace ExRam.Gremlinq.Core
{
    public sealed class SideEffectStep : Step
    {
        public SideEffectStep(Traversal traversal) : base()
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
