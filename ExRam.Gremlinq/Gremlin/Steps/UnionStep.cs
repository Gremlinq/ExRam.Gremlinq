using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public sealed class UnionStep : MultiTraversalArgumentStep
    {
        public UnionStep(IEnumerable<IGremlinQuery> traversals) : base(traversals)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
