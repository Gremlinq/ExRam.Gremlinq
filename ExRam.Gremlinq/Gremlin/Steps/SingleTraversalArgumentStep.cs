using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public abstract class SingleTraversalArgumentStep : NonTerminalStep
    {
        private readonly string _name;
        private readonly IGremlinQuery _traversal;

        protected SingleTraversalArgumentStep(string name, IGremlinQuery traversal)
        {
            _name = name;
            _traversal = traversal;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            yield return MethodStep.Create(_name, _traversal.Resolve(model));
        }
    }
}