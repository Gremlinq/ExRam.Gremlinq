namespace ExRam.Gremlinq.Core
{
    public sealed class OrStep : LogicalStep
    {
        public OrStep(IGremlinQuery[] traversals) : base("or", traversals)
        {
        }
    }
}
