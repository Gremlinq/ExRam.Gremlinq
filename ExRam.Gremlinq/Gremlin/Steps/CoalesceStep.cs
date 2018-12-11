using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public sealed class CoalesceStep : MultiTraversalArgumentStep
    {
        public CoalesceStep(IEnumerable<IGremlinQuery> traversals) : base(traversals)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
