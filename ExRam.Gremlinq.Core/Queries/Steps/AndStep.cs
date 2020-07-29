using System;
using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public sealed class AndStep : LogicalStep<AndStep>, IIsOptimizableInWhere
    {
        public static readonly AndStep Infix = new AndStep(Array.Empty<Traversal>());

        public AndStep(IEnumerable<Traversal> traversals) : base("and", traversals)
        {
        }
    }
}
