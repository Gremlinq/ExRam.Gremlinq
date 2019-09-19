using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public sealed class UnionStep : MultiTraversalArgumentStep
    {
        public UnionStep(IEnumerable<IGremlinQuery> traversals) : base(traversals)
        {
        }
    }
}
