using System;

namespace ExRam.Gremlinq.Core
{
    public sealed class AndStep : LogicalStep<AndStep>
    {
        public static readonly AndStep Infix = new AndStep(Array.Empty<Traversal>());

        public AndStep(Traversal[] traversals) : base("and", traversals)
        {
        }
    }
}
