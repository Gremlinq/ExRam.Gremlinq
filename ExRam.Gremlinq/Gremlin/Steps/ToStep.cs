namespace ExRam.Gremlinq
{
    public sealed class ToStep : SingleTraversalArgumentStep
    {
        public ToStep(IGremlinQuery traversal) : base("to", traversal)
        {
        }
    }
}