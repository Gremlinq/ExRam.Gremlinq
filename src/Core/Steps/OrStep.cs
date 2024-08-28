namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class OrStep : LogicalStep<OrStep>, IFilterStep
    {
        //TODO: Change to ImmutableArray
        public OrStep(IEnumerable<Traversal> traversals) : base("or", traversals)
        {
        }
    }
}
