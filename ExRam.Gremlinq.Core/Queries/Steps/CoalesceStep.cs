using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public sealed class CoalesceStep : MultiTraversalArgumentStep
    {
        public CoalesceStep(IEnumerable<IGremlinQuery> traversals) : base(traversals)
        {
        }
    }
}
