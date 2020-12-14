namespace ExRam.Gremlinq.Core
{
    public sealed class UntilStep : Step
    {
        public UntilStep(Traversal traversal)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
