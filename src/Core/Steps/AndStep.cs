using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class AndStep : LogicalStep<AndStep>, IFilterStep
    {
        [Obsolete("Will be removed in a future version. Use the overload taking an ImmutableArray<Traversal> instead.")]
        public AndStep(IEnumerable<Traversal> traversals) : base("and", traversals)
        {
        }

        public AndStep(ImmutableArray<Traversal> traversals) : base("and", traversals)
        {
        }
    }
}
