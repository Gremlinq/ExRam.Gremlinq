namespace ExRam.Gremlinq
{
    public sealed class OptionalStep : SingleTraversalArgumentStep
    {
        public OptionalStep(IGremlinQuery traversal) : base("optional", traversal)
        {
        }
    }
}