using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public sealed class MatchStep : MultiTraversalArgumentStep
    {
        public MatchStep(IEnumerable<IGremlinQueryBase> traversals) : base(traversals)
        {
        }
    }
}
