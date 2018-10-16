using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public abstract class NonTerminalStep : Step
    {
        public abstract IEnumerable<TerminalStep> Resolve(IGraphModel model);
    }
}