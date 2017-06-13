using System.Reflection;
using FluentAssertions;
using Xunit;

namespace ExRam.Gremlinq.Tests
{
    public class GraphSchemaTest
    {
        [Fact]
        public void FromAssembly_ToGraphSchema_does_not_include_abstract_vertex_types()
        {
            var schema = GraphModel
                .FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple)
                .ToGraphSchema();

            schema.VertexSchemaInfos
                .Should()
                .NotContain(x => x.Label == "Vertex");

            schema.VertexSchemaInfos
                .Should()
                .NotContain(x => x.Label == "Authority");
        }

        [Fact]
        public void FromAssembly_ToGraphSchema_does_not_include_abstract_edge_types()
        {
            var model = GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple);

            model.ToGraphSchema().EdgeSchemaInfos
                .Should()
                .NotContain(x => x.Label == "Edge");
        }
    }
}