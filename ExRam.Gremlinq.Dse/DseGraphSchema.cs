using System.Collections.Immutable;

namespace ExRam.Gremlinq.Dse
{
    public sealed class DseGraphSchema : IGraphSchema
    {
        public DseGraphSchema(IGraphModel model, ImmutableList<EdgeSchemaInfo> edgeSchemaInfos, ImmutableList<PropertySchemaInfo> propertySchemaInfos, ImmutableList<(string, string, string)> connections)
        {
            this.Model = model;
            this.EdgeSchemaInfos = edgeSchemaInfos;
            this.PropertySchemaInfos = propertySchemaInfos;
            this.Connections = connections;
        }

        public IGraphModel Model { get; }
        public ImmutableList<EdgeSchemaInfo> EdgeSchemaInfos { get; }
        public ImmutableList<PropertySchemaInfo> PropertySchemaInfos { get; }
        public ImmutableList<(string, string, string)> Connections { get; }
    }
}