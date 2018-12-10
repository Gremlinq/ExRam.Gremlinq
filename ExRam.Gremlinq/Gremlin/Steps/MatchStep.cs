using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public sealed class MatchStep : MultiTraversalArgumentStep
    {
        public MatchStep(IEnumerable<IGremlinQuery> traversals) : base("match", traversals)
        {
        }
    }
}
