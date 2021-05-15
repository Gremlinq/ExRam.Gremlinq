namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class ChooseOptionTraversalStep : Step
    {
        public ChooseOptionTraversalStep(Traversal traversal) : base()
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
