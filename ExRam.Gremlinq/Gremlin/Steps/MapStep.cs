namespace ExRam.Gremlinq
{
    public sealed class MapStep : SingleTraversalArgumentStep
    {
        public MapStep(IGremlinQuery traversal) : base("map", traversal)
        {
        }
    }
}