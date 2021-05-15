using System;
using System.Collections.Generic;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class AndStep : LogicalStep<AndStep>, IIsOptimizableInWhere
    {
        public static readonly AndStep Infix = new(Array.Empty<Traversal>());
        
        public AndStep(IEnumerable<Traversal> traversals) : base("and", traversals)
        {
        }
    }
}
