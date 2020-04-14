using System;

namespace ExRam.Gremlinq.Core
{
    public sealed class OrStep : LogicalStep<OrStep>
    {
        public static readonly OrStep Infix = new OrStep(Array.Empty<Traversal>());

        public OrStep(Traversal[] traversals) : base("or", traversals)
        {
        }
    }
}
