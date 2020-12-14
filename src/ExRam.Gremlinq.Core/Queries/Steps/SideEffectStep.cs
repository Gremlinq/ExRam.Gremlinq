namespace ExRam.Gremlinq.Core
{
    public sealed class SideEffectStep : Step
    {
        public SideEffectStep(Traversal traversal)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
