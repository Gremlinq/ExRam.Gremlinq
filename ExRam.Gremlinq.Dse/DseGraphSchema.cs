using System.Collections.Immutable;

namespace ExRam.Gremlinq.Dse
{
    public sealed class DseGraphSchema : IGraphSchema
    {
        public DseGraphSchema(IGraphModel model, ImmutableList<(string, string, string)> connections)
        {
            this.Model = model;
            this.Connections = connections;
        }

        public IGraphModel Model { get; }
        public ImmutableList<(string, string, string)> Connections { get; }
    }
}