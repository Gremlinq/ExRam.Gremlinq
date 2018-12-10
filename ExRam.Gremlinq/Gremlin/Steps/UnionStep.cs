using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public sealed class UnionStep : MultiTraversalArgumentStep
    {
        public UnionStep(IEnumerable<IGremlinQuery> traversals) : base("union", traversals)
        {
        }
    }
}