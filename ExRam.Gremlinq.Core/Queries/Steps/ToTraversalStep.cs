namespace ExRam.Gremlinq.Core
{
    public sealed class ToTraversalStep : SingleTraversalArgumentStep
    {
        public ToTraversalStep(IGremlinQuery traversal) : base(traversal)
        {
        }
    }
}
