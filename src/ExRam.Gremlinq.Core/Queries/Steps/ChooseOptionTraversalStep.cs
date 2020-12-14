namespace ExRam.Gremlinq.Core
{
    public sealed class ChooseOptionTraversalStep : Step
    {
        public ChooseOptionTraversalStep(Traversal traversal)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
