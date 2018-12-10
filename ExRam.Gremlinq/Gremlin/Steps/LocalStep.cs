namespace ExRam.Gremlinq
{
    public sealed class LocalStep : SingleTraversalArgumentStep
    {
        public LocalStep(IGremlinQuery traversal) : base("local", traversal)
        {
        }
    }
}