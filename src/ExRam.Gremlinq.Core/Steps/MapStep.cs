namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class MapStep : Step
    {
        public MapStep(Traversal traversal) : base(traversal.GetTraversalSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
