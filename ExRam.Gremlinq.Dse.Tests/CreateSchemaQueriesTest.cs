using System.Linq;
using ExRam.Gremlinq.Tests;
using Xunit;
using FluentAssertions;

namespace ExRam.Gremlinq.Dse.Tests
{
    public class DseGraphSchemaTest
    {
        private readonly IGremlinQuery<string>[] _queries;

        public DseGraphSchemaTest()
        {
            _queries = GraphModel
                .FromAssembly(typeof(GraphModelTest).Assembly, typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple)
                .ToDseGraphModel()
                .SecondaryIndex<Authority>(x => x.Name)
                .SecondaryIndex<TimeFrame>(x => x.StartTime)
                .MaterializedIndex<Authority>(x => x.PhoneNumbers)
                .SearchIndex<Country>(x => x.CountryCallingCode)
                .EdgeIndex<User, WorksFor>(x => x.From, EdgeDirection.Out)
                .AddConnection<Authority, IsDescribedIn, Language>()
                .AddConnection<User, WorksFor, Authority>()
                .AddConnection<User, Edge, User>()
                .CreateSchemaQueries()
                .ToArray();
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_includes_non_abstract_vertex_types()
        {
            _queries
                .Should()
                .Contain(x => x.Steps.OfType<MethodStep>().Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "User"));

            _queries
                .Should()
                .Contain(x => x.Steps.OfType<MethodStep>().Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "Company"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_includes_properties()
        {
            _queries
                .Should()
                .Contain(x => x.Steps.OfType<MethodStep>().Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "User") &&
                    x.Steps.OfType<MethodStep>().Any(step => step.Name == "properties" && step.Parameters.Contains("Name")));

            _queries
                .Should()
                .OnlyContain(x => x.Steps.OfType<MethodStep>().All(step => step.Name != "properties" || step.Parameters.All(y => y is string)));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_does_not_include_abstract_vertex_types()
        {
            _queries
                .Should()
                .NotContain(x => x.Steps.OfType<MethodStep>().Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "Vertex"));

            _queries
                .Should()
                .NotContain(x => x.Steps.OfType<MethodStep>().Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "Authority"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_includes_non_abstract_edge_types()
        {
            _queries
                .Should()
                .Contain(x => x.Steps.OfType<MethodStep>().Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "WorksFor"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_does_not_include_abstract_edge_types()
        {
            _queries
                .Should()
                .NotContain(x => x.Steps.OfType<MethodStep>().Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "Edge"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_includes_edge_connection_closure()
        {
            _queries
                .Should()
                .Contain(x => x.Steps.OfType<MethodStep>().Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "IsDescribedIn") &&
                    x.Steps.OfType<MethodStep>().Any(step => step.Name == "connection" && (string)step.Parameters[0] == "User" &&  (string)step.Parameters[1] == "Language"));

            _queries
                .Should()
                .Contain(x => x.Steps.OfType<MethodStep>().Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "IsDescribedIn") &&
                    x.Steps.OfType<MethodStep>().Any(step => step.Name == "connection" && (string)step.Parameters[0] == "Company" && (string)step.Parameters[1] == "Language"));

            _queries
                .Should()
                .Contain(x => x.Steps.OfType<MethodStep>().Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "WorksFor") &&
                    x.Steps.OfType<MethodStep>().Any(step => step.Name == "connection" && (string)step.Parameters[0] == "User" && (string)step.Parameters[1] == "User"));

            _queries
                .Should()
                .Contain(x => x.Steps.OfType<MethodStep>().Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "WorksFor") &&
                    x.Steps.OfType<MethodStep>().Any(step => step.Name == "connection" && (string)step.Parameters[0] == "User" && (string)step.Parameters[1] == "Company"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_does_not_include_connections_from_abstract_vertices()
        {
            _queries
                .Should()
                .NotContain(x => x.Steps.OfType<MethodStep>().Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "Authority"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_does_not_include_connections_to_abstract_vertices()
        {
            _queries
                .Should()
                .NotContain(x => x.Steps.OfType<MethodStep>().Any(step => step.Name == "connection" && (string)step.Parameters[1] == "Authority"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_does_not_include_connections_by_abstract_edges()
        {
            _queries
                .Should()
                .Contain(x => x.Steps.OfType<MethodStep>().Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "Knows") &&
                                 x.Steps.OfType<MethodStep>().Any(step => step.Name == "connection" && (string)step.Parameters[0] == "User" && (string)step.Parameters[1] == "User"));

            _queries
                .Should()
                .Contain(x => x.Steps.OfType<MethodStep>().Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "Speaks") &&
                                 x.Steps.OfType<MethodStep>().Any(step => step.Name == "connection" && (string)step.Parameters[0] == "User" && (string)step.Parameters[1] == "User"));

            _queries
                .Should()
                .Contain(x => x.Steps.OfType<MethodStep>().Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "WorksFor") &&
                                 x.Steps.OfType<MethodStep>().Any(step => step.Name == "connection" && (string)step.Parameters[0] == "User" && (string)step.Parameters[1] == "User"));

            _queries
                .Should()
                .NotContain(x => x.Steps.OfType<MethodStep>().Any(step => step.Name == "edgeLabel" && (string)step.Parameters[0] == "Edge"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_does_not_include_secondary_index_for_abstract_type()
        {
            _queries
                .Should()
                .NotContain(query => query.Steps.OfType<MethodStep>().Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "Authority") &&
                                    query.Steps.OfType<MethodStep>().Any(step => step.Name == "secondary") &&
                                    query.Steps.OfType<MethodStep>().Any(step => step.Name == "by" && (string)step.Parameters[0] == "Name"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_includes_secondary_index_for_inherited_type()
        {
            _queries
                .Should()
                .Contain(query => query.Steps.OfType<MethodStep>().Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "User") &&
                                  query.Steps.OfType<MethodStep>().Any(step => step.Name == "secondary") &&
                                  query.Steps.OfType<MethodStep>().Any(step => step.Name == "by" && (string)step.Parameters[0] == "Name"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_includes_secondary_index_for_value_type_expression()
        {
            _queries
                .Should()
                .Contain(query => query.Steps.OfType<MethodStep>().Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "TimeFrame") &&
                                  query.Steps.OfType<MethodStep>().Any(step => step.Name == "secondary") &&
                                  query.Steps.OfType<MethodStep>().Any(step => step.Name == "by" && (string)step.Parameters[0] == "StartTime"));
        }
        
        [Fact]
        public void FromAssembly_CreateSchemaQueries_does_not_include_materialized_index_for_abstract_type()
        {
            _queries
                .Should()
                .NotContain(query => query.Steps.OfType<MethodStep>().Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "Authority") &&
                                     query.Steps.OfType<MethodStep>().Any(step => step.Name == "materialized") &&
                                     query.Steps.OfType<MethodStep>().Any(step => step.Name == "by" && (string)step.Parameters[0] == "PhoneNumbers"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_includes_materialized_index_for_inherited_type()
        {
            _queries
                .Should()
                .Contain(query => query.Steps.OfType<MethodStep>().Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "User") &&
                                  query.Steps.OfType<MethodStep>().Any(step => step.Name == "materialized") &&
                                  query.Steps.OfType<MethodStep>().Any(step => step.Name == "by" && (string)step.Parameters[0] == "PhoneNumbers"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_includes_search_index()
        {
            _queries
                .Should()
                .Contain(query => query.Steps.OfType<MethodStep>().Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "Country") &&
                                  query.Steps.OfType<MethodStep>().Any(step => step.Name == "index" && (string)step.Parameters[0] == "search") &&
                                  query.Steps.OfType<MethodStep>().Any(step => step.Name == "search") &&
                                  query.Steps.OfType<MethodStep>().Any(step => step.Name == "by" && (string)step.Parameters[0] == "CountryCallingCode"));
        }

        [Fact]
        public void FromAssembly_CreateSchemaQueries_includes_edge_index()
        {
            _queries
                .Should()
                .Contain(query => query.Steps.OfType<MethodStep>().Any(step => step.Name == "vertexLabel" && (string)step.Parameters[0] == "User") &&
                                  query.Steps.OfType<MethodStep>().Any(step => step.Name == "index") &&
                                  query.Steps.OfType<MethodStep>().Any(step => step.Name == "outE" && (string)step.Parameters[0] == "WorksFor") &&
                                  query.Steps.OfType<MethodStep>().Any(step => step.Name == "by" && (string)step.Parameters[0] == "From"));
        }
    }
}
