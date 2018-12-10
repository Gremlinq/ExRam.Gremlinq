namespace ExRam.Gremlinq
{
    public sealed class FromStep : SingleTraversalArgumentStep
    {
        public FromStep(IGremlinQuery traversal) : base("from", traversal)
        {
        }
    }
}