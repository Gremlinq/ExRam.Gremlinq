using System;
using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public sealed class OrStep : LogicalStep<OrStep>, IIsOptimizableInWhere
    {
        public static readonly OrStep Infix = new(Array.Empty<Traversal>());

        public OrStep(IEnumerable<Traversal> traversals, QuerySemantics? semantics = default) : base("or", traversals, semantics)
        {
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new OrStep(Traversals, semantics);
    }
}
