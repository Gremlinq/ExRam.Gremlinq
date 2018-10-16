using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq
{
    public sealed class SetQueryProviderStep : NonTerminalStep
    {
        public SetQueryProviderStep(IQueryProvider queryProvider)
        {
            QueryProvider = queryProvider;
        }

        public override IEnumerable<TerminalStep> Resolve(IGraphModel model)
        {
            return Enumerable.Empty<TerminalStep>();
        }

        public IQueryProvider QueryProvider { get; }
    }
}
