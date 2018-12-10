namespace ExRam.Gremlinq
{
    public sealed class EdgesStep : SingleTraversalArgumentStep
    {
        public EdgesStep(IGremlinQuery traversal) : base("edges", traversal)
        {
        }
    }
}