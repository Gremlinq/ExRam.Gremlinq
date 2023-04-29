namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class AndStep : LogicalStep<AndStep>, IIsOptimizableInWhere
    {
        public AndStep(IEnumerable<Traversal> traversals) : base("and", traversals)
        {
        }
    }
}
