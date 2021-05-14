namespace ExRam.Gremlinq.Core
{
    public sealed class FlatMapStep : Step
    {
        public FlatMapStep(Traversal traversal) : base()
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
