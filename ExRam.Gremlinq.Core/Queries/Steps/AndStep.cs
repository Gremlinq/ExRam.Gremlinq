namespace ExRam.Gremlinq.Core
{
    public sealed class AndStep : LogicalStep
    {
        public AndStep(IGremlinQuery[] traversals) : base("and", traversals)
        {
        }
    }
}
