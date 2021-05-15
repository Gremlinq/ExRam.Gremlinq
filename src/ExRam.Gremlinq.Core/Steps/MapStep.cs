namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class MapStep : Step
    {
        public MapStep(Traversal traversal) : base()
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
