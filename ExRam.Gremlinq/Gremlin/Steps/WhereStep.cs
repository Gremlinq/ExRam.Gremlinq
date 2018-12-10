namespace ExRam.Gremlinq
{
    public sealed class WhereStep : SingleTraversalArgumentStep
    {
        public WhereStep(IGremlinQuery traversal) : base("where", traversal)
        {
        }
    }
}