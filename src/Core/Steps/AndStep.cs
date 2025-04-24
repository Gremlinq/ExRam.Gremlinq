using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class AndStep : LogicalStep<AndStep>, IFilterStep
    {
        public AndStep(ImmutableArray<Traversal> traversals) : base("and", traversals)
        {
        }
    }
}
