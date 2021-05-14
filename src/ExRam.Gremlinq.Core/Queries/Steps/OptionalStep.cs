namespace ExRam.Gremlinq.Core
{
    public sealed class OptionalStep : Step
    {
        public OptionalStep(Traversal traversal) : base()
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
