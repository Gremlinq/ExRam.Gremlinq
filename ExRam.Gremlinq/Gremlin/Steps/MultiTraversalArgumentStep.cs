using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public abstract class MultiTraversalArgumentStep : Step
    {
        public IEnumerable<IGremlinQuery> Traversals { get; }

        protected MultiTraversalArgumentStep(IEnumerable<IGremlinQuery> traversals)
        {
            Traversals = traversals;
        }
    }
}
