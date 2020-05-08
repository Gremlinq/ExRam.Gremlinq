namespace ExRam.Gremlinq.Core
{
    public sealed class LocalStep : Step
    {
        public LocalStep(Traversal traversal)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
