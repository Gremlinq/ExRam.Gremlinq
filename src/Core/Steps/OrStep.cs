using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class OrStep : LogicalStep<OrStep>, IFilterStep
    {
        [Obsolete("Will be removed in a future version. Use the overload taking an ImmutableArray<Traversal> instead.")]
        public OrStep(IEnumerable<Traversal> traversals) : base("or", traversals)
        {
        }

        public OrStep(ImmutableArray<Traversal> traversals) : base("or", traversals)
        {
        }
    }
}
