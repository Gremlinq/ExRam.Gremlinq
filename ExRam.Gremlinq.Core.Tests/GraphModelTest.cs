using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Xunit;
using LanguageExt;
using System.Linq.Expressions;
using System;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GraphModelTest
    {
        private sealed class VertexOutsideHierarchy
        {
            public object Id { get; set; }
        }

        private sealed class VertexInsideHierarchy : Vertex
        {
        }

        [Fact]
        public void TryGetFilterLabels_does_not_include_abstract_type()
        {
            var model = GraphModel.Dynamic();

            model.VerticesModel.TryGetFilterLabels(typeof(Authority))
                .IfNone(new string[0])
                .Should()
                .Contain("Company").And
                .Contain("Person").And
                .NotContain("Authority");
        }

        [Fact]
        public void No_Relax_in_hierarchy_inside_model()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .VerticesModel
                .TryGetConstructiveLabel(typeof(Person))
                .Should()
                .BeSome("Person");
        }

        [Fact]
        public void No_Relax_in_hierarchy_outside_model()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .VerticesModel
                .TryGetConstructiveLabel(typeof(VertexInsideHierarchy))
                .Should()
                .BeNone();
        }

        [Fact]
        public void No_Relax_outside_hierarchy()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .VerticesModel
                .TryGetConstructiveLabel(typeof(VertexOutsideHierarchy))
                .Should()
                .BeNone();
        }

        [Fact]
        public void Relax_in_hierarchy_inside_model()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>().Relax()
                .VerticesModel
                .TryGetConstructiveLabel(typeof(Person))
                .Should()
                .BeSome("Person");
        }

        [Fact]
        public void Relax_in_hierarchy_outside_model()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>().Relax()
                .VerticesModel
                .TryGetConstructiveLabel(typeof(VertexInsideHierarchy))
                .Should()
                .BeSome("VertexInsideHierarchy");
        }

        [Fact]
        public void Relax_outside_hierarchy()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>().Relax()
                .VerticesModel
                .TryGetConstructiveLabel(typeof(VertexOutsideHierarchy))
                .Should()
                .BeSome("VertexOutsideHierarchy");
        }

        [Fact]
        public void Lowercase()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithLowercaseLabels()
                .VerticesModel
                .TryGetConstructiveLabel(typeof(Person))
                .Should()
                .BeSome("person");
        }

        [Fact]
        public void Camelcase_Verticies()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelcase()
                .VerticesModel
                .TryGetConstructiveLabel(typeof(TimeFrame))
                .Should()
                .BeEqual("timeFrame");
        }

        [Fact]
        public void Camelcase_Edges()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelcase()
                .EdgesModel
                .TryGetConstructiveLabel(typeof(LivesIn))
                .Should()
                .BeEqual("livesIn");
        }

        [Fact]
        public void Camelcase_Identifier_By_Type()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelcase()
                .GetIdentifier(typeof(Person), nameof(Person.RegistrationDate))
                .Should()
                .Be("registrationDate");
        }

        [Fact]
        public void Camelcase_Identifier_By_MemberExpression()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelcase()
                .GetIdentifier(Expression.Property(Expression.Constant(new Person()), nameof(Person.RegistrationDate)))
                .Should()
                .Be("registrationDate");
        }

        [Fact]
        public void Camelcase_Identifier_By_ParameterExpression()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelcase()
                .GetIdentifier(Expression.Parameter(typeof(Person), nameof(Person.RegistrationDate)))
                .Should()
                .Be("registrationDate");
        }
    }
}
