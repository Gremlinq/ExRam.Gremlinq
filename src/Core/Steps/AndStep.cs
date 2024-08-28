namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class AndStep : LogicalStep<AndStep>, IFilterStep
    {
        //TODO: Change to ImmutableArray
        public AndStep(IEnumerable<Traversal> traversals) : base("and", traversals)
        {
        }
    }
}
