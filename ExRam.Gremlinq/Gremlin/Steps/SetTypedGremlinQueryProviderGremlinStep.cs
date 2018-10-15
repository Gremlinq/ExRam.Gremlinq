using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq
{
    public sealed class SetTypedGremlinQueryProviderGremlinStep : NonTerminalGremlinStep
    {
        public SetTypedGremlinQueryProviderGremlinStep(IGremlinQueryProvider gremlinQueryProvider)
        {
            GremlinQueryProvider = gremlinQueryProvider;
        }

        public override IEnumerable<TerminalGremlinStep> Resolve(IGraphModel model)
        {
            return Enumerable.Empty<TerminalGremlinStep>();
        }

        public IGremlinQueryProvider GremlinQueryProvider { get; }
    }
}