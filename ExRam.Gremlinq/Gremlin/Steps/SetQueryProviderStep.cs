using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq
{
    public sealed class SetQueryProviderStep : NonTerminalStep
    {
        public SetQueryProviderStep(IGremlinQueryProvider gremlinQueryProvider)
        {
            GremlinQueryProvider = gremlinQueryProvider;
        }

        public override IEnumerable<TerminalStep> Resolve(IGraphModel model)
        {
            return Enumerable.Empty<TerminalStep>();
        }

        public IGremlinQueryProvider GremlinQueryProvider { get; }
    }
}
