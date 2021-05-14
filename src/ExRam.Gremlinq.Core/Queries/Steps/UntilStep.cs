namespace ExRam.Gremlinq.Core
{
    public sealed class UntilStep : Step
    {
        public UntilStep(Traversal traversal) : base()
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
