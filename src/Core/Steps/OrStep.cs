using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class OrStep : LogicalStep<OrStep>, IFilterStep
    {
        public OrStep(ImmutableArray<Traversal> traversals) : base(traversals)
        {
        }
    }
}
