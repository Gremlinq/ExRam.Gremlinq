using System.Linq;
using ExRam.Gremlinq.Tests;
using Xunit;
using FluentAssertions;
using Moq;

namespace ExRam.Gremlinq.Dse.Tests
{
    public class DseGraphSchemaTest
    {
        [Fact]
        public void FromAssembly_CreateSchemaQueries_includes_non_abstract_vertex_types()
        {
            var queries = GraphModel
                .FromAssembly(typeof(ExRam.Gremlinq.Tests.Vertex).Assembly, typeof(ExRam.Gremlinq.Tests.Vertex), typeof(ExRam.Gremlinq.Tests.Edge), GraphElementNamingStrategy.Simple)
                .ToDseGraphModel()
                .CreateSchemaQueries(Mock.Of<IGremlinQueryProvider>())
                .ToArray();

            queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "User"));

            queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "Company"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_includes_properties()
        {
            var queries = GraphModel
                .FromAssembly(typeof(ExRam.Gremlinq.Tests.Vertex).Assembly, typeof(ExRam.Gremlinq.Tests.Vertex), typeof(ExRam.Gremlinq.Tests.Edge), GraphElementNamingStrategy.Simple)
                .ToDseGraphModel()
                .CreateSchemaQueries(Mock.Of<IGremlinQueryProvider>())
                .ToArray();

            queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "User") &&
                    x.Steps.Any(step => step.Name == "properties" && step.Parameters.Contains("Name")));

            queries
                .Should()
                .OnlyContain(x => x.Steps.All(step => step.Name != "properties" || step.Parameters.All(y => y is string)));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_does_not_include_abstract_vertex_types()
        {
            var queries = GraphModel
                .FromAssembly(typeof(ExRam.Gremlinq.Tests.Vertex).Assembly, typeof(ExRam.Gremlinq.Tests.Vertex), typeof(ExRam.Gremlinq.Tests.Edge), GraphElementNamingStrategy.Simple)
                .ToDseGraphModel()
                .CreateSchemaQueries(Mock.Of<IGremlinQueryProvider>())
                .ToArray();

            queries
                .Should()
                .NotContain(x => x.Steps.Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "Vertex"));

            queries
                .Should()
                .NotContain(x => x.Steps.Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "Authority"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_includes_non_abstract_edge_types()
        {
            GraphModel
                .FromAssembly(typeof(ExRam.Gremlinq.Tests.Vertex).Assembly, typeof(ExRam.Gremlinq.Tests.Vertex), typeof(ExRam.Gremlinq.Tests.Edge), GraphElementNamingStrategy.Simple)
                .ToDseGraphModel()
                .CreateSchemaQueries(Mock.Of<IGremlinQueryProvider>())
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "WorksFor"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_does_not_include_abstract_edge_types()
        {
            GraphModel
                .FromAssembly(typeof(ExRam.Gremlinq.Tests.Vertex).Assembly, typeof(ExRam.Gremlinq.Tests.Vertex), typeof(ExRam.Gremlinq.Tests.Edge), GraphElementNamingStrategy.Simple)
                .ToDseGraphModel()
                .CreateSchemaQueries(Mock.Of<IGremlinQueryProvider>())
                .Should()
                .NotContain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "Edge"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_includes_edge_connection_closure()
        {
            var queries = GraphModel
                .FromAssembly(typeof(ExRam.Gremlinq.Tests.Vertex).Assembly, typeof(ExRam.Gremlinq.Tests.Vertex), typeof(ExRam.Gremlinq.Tests.Edge), GraphElementNamingStrategy.Simple)
                .ToDseGraphModel()
                .AddConnection<Authority, IsDescribedIn, Language>()
                .AddConnection<User, WorksFor, Authority>()
                .CreateSchemaQueries(Mock.Of<IGremlinQueryProvider>())
                .ToArray();

            queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "IsDescribedIn") &&
                    x.Steps.Any(step => step.Name == "connection" && (string)step.Parameters[0] == "User" &&  (string)step.Parameters[1] == "Language"));

            queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "IsDescribedIn") &&
                    x.Steps.Any(step => step.Name == "connection" && (string)step.Parameters[0] == "Company" && (string)step.Parameters[1] == "Language"));

            queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "WorksFor") &&
                    x.Steps.Any(step => step.Name == "connection" && (string)step.Parameters[0] == "User" && (string)step.Parameters[1] == "User"));

            queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "WorksFor") &&
                    x.Steps.Any(step => step.Name == "connection" && (string)step.Parameters[0] == "User" && (string)step.Parameters[1] == "Company"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_does_not_include_connections_from_abstract_vertices()
        {
            GraphModel
                .FromAssembly(typeof(ExRam.Gremlinq.Tests.Vertex).Assembly, typeof(ExRam.Gremlinq.Tests.Vertex), typeof(ExRam.Gremlinq.Tests.Edge), GraphElementNamingStrategy.Simple)
                .ToDseGraphModel()
                .AddConnection<Authority, IsDescribedIn, Language>()
                .CreateSchemaQueries(Mock.Of<IGremlinQueryProvider>())
                .Should()
                .NotContain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "Authority"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_does_not_include_connections_to_abstract_vertices()
        {
            GraphModel
                .FromAssembly(typeof(ExRam.Gremlinq.Tests.Vertex).Assembly, typeof(ExRam.Gremlinq.Tests.Vertex), typeof(ExRam.Gremlinq.Tests.Edge), GraphElementNamingStrategy.Simple)
                .ToDseGraphModel()
                .AddConnection<User, WorksFor, Authority>()
                .CreateSchemaQueries(Mock.Of<IGremlinQueryProvider>())
                .Should()
                .NotContain(x => x.Steps.Any(step => step.Name == "connection" && (string)step.Parameters[1] == "Authority"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_does_not_include_connections_by_abstract_edges()
        {
            var queries = GraphModel
                .FromAssembly(typeof(ExRam.Gremlinq.Tests.Vertex).Assembly, typeof(ExRam.Gremlinq.Tests.Vertex), typeof(ExRam.Gremlinq.Tests.Edge), GraphElementNamingStrategy.Simple)
                .ToDseGraphModel()
                .AddConnection<User, ExRam.Gremlinq.Tests.Edge, User>()
                .CreateSchemaQueries(Mock.Of<IGremlinQueryProvider>())
                .ToArray();

            queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "Knows") &&
                                 x.Steps.Any(step => step.Name == "connection" && (string)step.Parameters[0] == "User" && (string)step.Parameters[1] == "User"));

            queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "Speaks") &&
                                 x.Steps.Any(step => step.Name == "connection" && (string)step.Parameters[0] == "User" && (string)step.Parameters[1] == "User"));

            queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "WorksFor") &&
                                 x.Steps.Any(step => step.Name == "connection" && (string)step.Parameters[0] == "User" && (string)step.Parameters[1] == "User"));

            queries
                .Should()
                .NotContain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "Edge"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_includes_index_of_base_types()
        {
            var queries = GraphModel
                .FromAssembly(typeof(ExRam.Gremlinq.Tests.Vertex).Assembly, typeof(ExRam.Gremlinq.Tests.Vertex), typeof(ExRam.Gremlinq.Tests.Edge), GraphElementNamingStrategy.Simple)
                .VertexLabel<Authority>()
                .ToDseGraphModel()
                .SecondaryIndex<Authority>(x => x.Name)
                .CreateSchemaQueries(Mock.Of<IGremlinQueryProvider>());

            queries
                .Should()
                .Contain(query => query.Steps.Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "Authority") &&
                    query.Steps.Any(step => step.Name == "secondary") &&
                    query.Steps.Any(step => step.Name == "by" && (string)step.Parameters[0] == "Name"));
        }
    }
}