using System.Collections.Immutable;
using ExRam.Gremlinq;

namespace Dse
{
    public class DseGraphSchema : IGraphSchema
    {
        private readonly IGraphSchema _graphSchemaImplementation;

        public DseGraphSchema(IGraphSchema graphSchemaImplementation)
        {
            this._graphSchemaImplementation = graphSchemaImplementation;
        }

        public IGraphModel Model
        {
            get { return this._graphSchemaImplementation.Model; }
        }

        public ImmutableList<VertexSchemaInfo> VertexSchemaInfos
        {
            get { return this._graphSchemaImplementation.VertexSchemaInfos; }
        }

        public ImmutableList<EdgeSchemaInfo> EdgeSchemaInfos
        {
            get { return this._graphSchemaImplementation.EdgeSchemaInfos; }
        }

        public ImmutableList<PropertySchemaInfo> PropertySchemaInfos
        {
            get { return this._graphSchemaImplementation.PropertySchemaInfos; }
        }

        public ImmutableList<(string, string, string)> Connections
        {
            get { return this._graphSchemaImplementation.Connections; }
        }
    }
}