using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public sealed class CoalesceStep : MultiTraversalArgumentStep
    {
        public CoalesceStep(IEnumerable<IGremlinQueryBase> traversals) : base(traversals)
        {
        }
    }
}
