namespace ExRam.Gremlinq.Core.Steps
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
