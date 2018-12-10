namespace ExRam.Gremlinq
{
    public sealed class WithStrategiesStep : SingleTraversalArgumentStep
    {
        public WithStrategiesStep(IGremlinQuery traversal) : base("withStrategies", traversal)
        {
        }
    }
}