using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public sealed class CoalesceStep : MultiTraversalArgumentStep
    {
        public CoalesceStep(IEnumerable<IGremlinQuery> traversals) : base("coalesce", traversals)
        {
        }
    }
}