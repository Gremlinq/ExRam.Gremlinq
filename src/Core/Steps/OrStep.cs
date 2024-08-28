namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class OrStep : LogicalStep<OrStep>, IFilterStep
    {
        public OrStep(IEnumerable<Traversal> traversals) : base("or", traversals)
        {
        }
    }
}
