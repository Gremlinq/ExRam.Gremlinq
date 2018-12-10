namespace ExRam.Gremlinq
{
    public sealed class UntilStep : SingleTraversalArgumentStep
    {
        public UntilStep(IGremlinQuery traversal) : base("until", traversal)
        {
        }
    }
}