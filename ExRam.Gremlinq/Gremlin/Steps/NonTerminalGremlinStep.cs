using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public abstract class NonTerminalGremlinStep : GremlinStep
    {
        public abstract IEnumerable<TerminalGremlinStep> Resolve(IGraphModel model);
    }
}