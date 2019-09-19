using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public sealed class MatchStep : MultiTraversalArgumentStep
    {
        public MatchStep(IEnumerable<IGremlinQuery> traversals) : base(traversals)
        {
        }
    }
}
