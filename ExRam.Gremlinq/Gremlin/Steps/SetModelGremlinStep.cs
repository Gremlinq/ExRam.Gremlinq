using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq
{
    public sealed class SetModelGremlinStep : NonTerminalGremlinStep
    {
        public SetModelGremlinStep(IGraphModel model)
        {
            this.Model = model;
        }

        public override IEnumerable<TerminalGremlinStep> Resolve(IGraphModel model)
        {
            return Enumerable.Empty<TerminalGremlinStep>();
        }

        public IGraphModel Model { get; }
    }
}