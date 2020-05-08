namespace ExRam.Gremlinq.Core
{
    public sealed class FlatMapStep : Step
    {
        public FlatMapStep(Traversal traversal)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
