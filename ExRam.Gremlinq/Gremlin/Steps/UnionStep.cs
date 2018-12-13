using System.Collections.Generic;
using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class UnionStep : MultiTraversalArgumentStep
    {
        public UnionStep(IEnumerable<IGremlinQuery> traversals) : base(traversals)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
