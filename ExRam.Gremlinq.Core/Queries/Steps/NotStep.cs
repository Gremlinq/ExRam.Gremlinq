namespace ExRam.Gremlinq.Core
{
    public sealed class NotStep : Step
    {
        public NotStep(Traversal traversal)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
