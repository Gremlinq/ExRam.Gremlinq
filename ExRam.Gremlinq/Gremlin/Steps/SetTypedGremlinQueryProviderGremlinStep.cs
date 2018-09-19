using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq
{
    public sealed class SetTypedGremlinQueryProviderGremlinStep : NonTerminalGremlinStep
    {
        public SetTypedGremlinQueryProviderGremlinStep(ITypedGremlinQueryProvider typedGremlinQueryProvider)
        {
            this.TypedGremlinQueryProvider = typedGremlinQueryProvider;
        }

        public override IEnumerable<TerminalGremlinStep> Resolve(IGraphModel model)
        {
            return Enumerable.Empty<TerminalGremlinStep>();
        }

        public ITypedGremlinQueryProvider TypedGremlinQueryProvider { get; }
    }
}