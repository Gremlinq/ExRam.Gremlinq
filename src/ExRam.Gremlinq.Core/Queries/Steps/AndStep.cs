using System;
using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public sealed class AndStep : LogicalStep<AndStep>, IIsOptimizableInWhere
    {
        public static readonly AndStep Infix = new(Array.Empty<Traversal>());
        
        public AndStep(IEnumerable<Traversal> traversals, QuerySemantics? semantics = default) : base("and", traversals, semantics)
        {
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new AndStep(Traversals, semantics);
    }
}
