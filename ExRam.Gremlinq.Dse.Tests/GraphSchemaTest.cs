using ExRam.Gremlinq.Tests;
using Xunit;
using FluentAssertions;

namespace ExRam.Gremlinq.Dse.Tests
{
    public class DseGraphSchemaTest
    {
        [Fact]
        public void FromAssembly_ToGraphSchema_does_not_include_abstract_vertex_types()
        {
            var schema = GraphModel
                .FromAssembly(typeof(ExRam.Gremlinq.Tests.Vertex).Assembly, typeof(ExRam.Gremlinq.Tests.Vertex), typeof(ExRam.Gremlinq.Tests.Edge), GraphElementNamingStrategy.Simple)
                .ToGraphSchema();

            schema.VertexSchemaInfos
                .Should()
                .NotContain(x => x.TypeInfo.Label == "Vertex");

            schema.VertexSchemaInfos
                .Should()
                .NotContain(x => x.TypeInfo.Label == "Authority");
        }

        [Fact]
        public void FromAssembly_ToGraphSchema_does_not_include_abstract_edge_types()
        {
            var model = GraphModel.FromAssembly(typeof(ExRam.Gremlinq.Tests.Vertex).Assembly, typeof(ExRam.Gremlinq.Tests.Vertex), typeof(ExRam.Gremlinq.Tests.Edge), GraphElementNamingStrategy.Simple);

            model.ToGraphSchema().EdgeSchemaInfos
                .Should()
                .NotContain(x => x.Label == "Edge");
        }

        [Fact]
        public void FromAssembly_ToGraphSchema_includes_edge_connection_closure()
        {
            var schema = GraphModel
                .FromAssembly(typeof(ExRam.Gremlinq.Tests.Vertex).Assembly, typeof(ExRam.Gremlinq.Tests.Vertex), typeof(ExRam.Gremlinq.Tests.Edge), GraphElementNamingStrategy.Simple)
                .AddConnection<Authority, IsDescribedIn, Language>()
                .AddConnection<User, WorksFor, Authority>()
                .ToGraphSchema();

            schema.Connections
                .Should()
                .Contain(("User", "IsDescribedIn", "Language"));

            schema.Connections
                .Should()
                .Contain(("Company", "IsDescribedIn", "Language"));

            schema.Connections
                .Should()
                .Contain(("User", "WorksFor", "User"));

            schema.Connections
                .Should()
                .Contain(("User", "WorksFor", "Company"));
        }

        [Fact]
        public void FromAssembly_ToGraphSchema_does_not_include_connections_from_abstract_vertices()
        {
            var schema = GraphModel
                .FromAssembly(typeof(ExRam.Gremlinq.Tests.Vertex).Assembly, typeof(ExRam.Gremlinq.Tests.Vertex), typeof(ExRam.Gremlinq.Tests.Edge), GraphElementNamingStrategy.Simple)
                .AddConnection<Authority, IsDescribedIn, Language>()
                .ToGraphSchema();

            schema.Connections
                .Should()
                .NotContain(tuple => tuple.Item1 == "Authority");
        }

        [Fact]
        public void FromAssembly_ToGraphSchema_does_not_include_connections_to_abstract_vertices()
        {
            var schema = GraphModel
                .FromAssembly(typeof(ExRam.Gremlinq.Tests.Vertex).Assembly, typeof(ExRam.Gremlinq.Tests.Vertex), typeof(ExRam.Gremlinq.Tests.Edge), GraphElementNamingStrategy.Simple)
                .AddConnection<User, WorksFor, Authority>()
                .ToGraphSchema();

            schema.Connections
                .Should()
                .NotContain(tuple => tuple.Item3 == "Authority");
        }

        [Fact]
        public void FromAssembly_ToGraphSchema_does_not_include_connections_by_abstract_edges()
        {
            var schema = GraphModel
                .FromAssembly(typeof(ExRam.Gremlinq.Tests.Vertex).Assembly, typeof(ExRam.Gremlinq.Tests.Vertex), typeof(ExRam.Gremlinq.Tests.Edge), GraphElementNamingStrategy.Simple)
                .AddConnection<User, ExRam.Gremlinq.Tests.Edge, User>()
                .ToGraphSchema();

            schema.Connections
                .Should()
                .Contain(("User", "Knows", "User"));

            schema.Connections
                .Should()
                .Contain(("User", "Speaks", "User"));

            schema.Connections
                .Should()
                .Contain(("User", "WorksFor", "User"));

            schema.Connections
                .Should()
                .NotContain(tuple => tuple.Item2 == "Edge");
        }

        [Fact]
        public void FromAssembly_ToGraphSchema_includes_index_of_base_types()
        {
            var schema = GraphModel
                .FromAssembly(typeof(ExRam.Gremlinq.Tests.Vertex).Assembly, typeof(ExRam.Gremlinq.Tests.Vertex), typeof(ExRam.Gremlinq.Tests.Edge), GraphElementNamingStrategy.Simple)
                .VertexType<Authority>(b => b.SecondaryIndex(x => x.Name))
                .ToGraphSchema();

            schema.VertexSchemaInfos
                .Should()
                .Contain(x => x.TypeInfo.Label == "User" && x.IndexProperties.Contains("Name"));
        }
    }
}