using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public abstract class MultiTraversalArgumentStep : Step
    {
        public IEnumerable<IGremlinQueryBase> Traversals { get; }

        protected MultiTraversalArgumentStep(IEnumerable<IGremlinQueryBase> traversals)
        {
            Traversals = traversals;
        }
    }
}
