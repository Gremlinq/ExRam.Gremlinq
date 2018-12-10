namespace ExRam.Gremlinq
{
    public sealed class MatchStep : SingleTraversalArgumentStep
    {
        public MatchStep(IGremlinQuery traversal) : base("match", traversal)
        {
        }
    }
}