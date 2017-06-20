using System.Linq;
using ExRam.Gremlinq.Tests;
using Xunit;
using FluentAssertions;
using Moq;

namespace ExRam.Gremlinq.Dse.Tests
{
    public class DseGraphSchemaTest
    {
        private readonly IGremlinQuery<string>[] _queries;

        public DseGraphSchemaTest()
        {
            this._queries = GraphModel
                .FromAssembly(typeof(Gremlinq.Tests.Vertex).Assembly, typeof(Gremlinq.Tests.Vertex), typeof(Gremlinq.Tests.Edge), GraphElementNamingStrategy.Simple)
                .ToDseGraphModel()
                .SecondaryIndex<Authority>(x => x.Name)
                .AddConnection<Authority, IsDescribedIn, Language>()
                .AddConnection<User, WorksFor, Authority>()
                .AddConnection<User, Gremlinq.Tests.Edge, User>()
                .CreateSchemaQueries(Mock.Of<IGremlinQueryProvider>())
                .ToArray();
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_includes_non_abstract_vertex_types()
        {
            this._queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "User"));

            this._queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "Company"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_includes_properties()
        {
            this._queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "User") &&
                    x.Steps.Any(step => step.Name == "properties" && step.Parameters.Contains("Name")));

            this._queries
                .Should()
                .OnlyContain(x => x.Steps.All(step => step.Name != "properties" || step.Parameters.All(y => y is string)));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_does_not_include_abstract_vertex_types()
        {
            this._queries
                .Should()
                .NotContain(x => x.Steps.Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "Vertex"));

            this._queries
                .Should()
                .NotContain(x => x.Steps.Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "Authority"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_includes_non_abstract_edge_types()
        {
            this._queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "WorksFor"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_does_not_include_abstract_edge_types()
        {
            this._queries
                .Should()
                .NotContain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "Edge"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_includes_edge_connection_closure()
        {
            this._queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "IsDescribedIn") &&
                    x.Steps.Any(step => step.Name == "connection" && (string)step.Parameters[0] == "User" &&  (string)step.Parameters[1] == "Language"));

            this._queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "IsDescribedIn") &&
                    x.Steps.Any(step => step.Name == "connection" && (string)step.Parameters[0] == "Company" && (string)step.Parameters[1] == "Language"));

            this._queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "WorksFor") &&
                    x.Steps.Any(step => step.Name == "connection" && (string)step.Parameters[0] == "User" && (string)step.Parameters[1] == "User"));

            this._queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "WorksFor") &&
                    x.Steps.Any(step => step.Name == "connection" && (string)step.Parameters[0] == "User" && (string)step.Parameters[1] == "Company"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_does_not_include_connections_from_abstract_vertices()
        {
            this._queries
                .Should()
                .NotContain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "Authority"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_does_not_include_connections_to_abstract_vertices()
        {
            this._queries
                .Should()
                .NotContain(x => x.Steps.Any(step => step.Name == "connection" && (string)step.Parameters[1] == "Authority"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_does_not_include_connections_by_abstract_edges()
        {
            this._queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "Knows") &&
                                 x.Steps.Any(step => step.Name == "connection" && (string)step.Parameters[0] == "User" && (string)step.Parameters[1] == "User"));

            this._queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "Speaks") &&
                                 x.Steps.Any(step => step.Name == "connection" && (string)step.Parameters[0] == "User" && (string)step.Parameters[1] == "User"));

            this._queries
                .Should()
                .Contain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "WorksFor") &&
                                 x.Steps.Any(step => step.Name == "connection" && (string)step.Parameters[0] == "User" && (string)step.Parameters[1] == "User"));

            this._queries
                .Should()
                .NotContain(x => x.Steps.Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "Edge"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_includes_secondary_index_for_inherited_type()
        {
            this._queries
                .Should()
                .Contain(query => query.Steps.Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "Authority") &&
                                  query.Steps.Any(step => step.Name == "secondary") &&
                                  query.Steps.Any(step => step.Name == "by" && (string)step.Parameters[0] == "Name"));

            this._queries
                .Should()
                .Contain(query => query.Steps.Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "User") &&
                                  query.Steps.Any(step => step.Name == "secondary") &&
                                  query.Steps.Any(step => step.Name == "by" && (string)step.Parameters[0] == "Name"));
        }
    }
}