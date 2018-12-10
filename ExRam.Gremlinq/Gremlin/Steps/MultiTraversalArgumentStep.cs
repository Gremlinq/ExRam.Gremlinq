using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq
{
    public abstract class MultiTraversalArgumentStep : NonTerminalStep
    {
        private readonly string _name;
        private readonly IEnumerable<IGremlinQuery> _traversals;

        protected MultiTraversalArgumentStep(string name, IEnumerable<IGremlinQuery> traversals)
        {
            _name = name;
            _traversals = traversals;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            yield return MethodStep.Create(_name, _traversals.Select(x => (object)x.Resolve(model)));
        }
    }
}
