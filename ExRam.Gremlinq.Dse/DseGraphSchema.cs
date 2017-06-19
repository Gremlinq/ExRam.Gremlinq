using System.Collections.Immutable;

namespace ExRam.Gremlinq.Dse
{
    public sealed class DseGraphSchema : IGraphSchema
    {
        public DseGraphSchema(IGraphModel model)
        {
            this.Model = model;
        }

        public IGraphModel Model { get; }
    }
}