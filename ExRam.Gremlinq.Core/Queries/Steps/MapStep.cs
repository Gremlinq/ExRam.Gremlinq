namespace ExRam.Gremlinq.Core
{
    public sealed class MapStep : Step
    {
        public MapStep(Traversal traversal)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
