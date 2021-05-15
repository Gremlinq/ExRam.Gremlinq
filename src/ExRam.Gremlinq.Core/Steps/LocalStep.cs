namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class LocalStep : Step
    {
        public LocalStep(Traversal traversal) : base()
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
