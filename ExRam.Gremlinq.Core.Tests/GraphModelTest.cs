using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Xunit;
using LanguageExt;
using System.Linq.Expressions;
using System;
using System.Linq;

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
                .Labels
                .TryGetValue(typeof(Person))
                .Should()
                .BeSome("Person");
        }

        [Fact]
        public void No_Relax_in_hierarchy_outside_model()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .VerticesModel
                .Labels
                .TryGetValue(typeof(VertexInsideHierarchy))
                .Should()
                .BeNone();
        }

        [Fact]
        public void No_Relax_outside_hierarchy()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .VerticesModel
                .Labels
                .TryGetValue(typeof(VertexOutsideHierarchy))
                .Should()
                .BeNone();
        }

        [Fact]
        public void Lowercase()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithLowercaseLabels()
                .VerticesModel
                .Labels
                .TryGetValue(typeof(Person))
                .Should()
                .BeSome("person");
        }

        [Fact]
        public void CamelcaseLabel_Verticies()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelcaseLabels()
                .VerticesModel
                .Labels
                .TryGetValue(typeof(TimeFrame))
                .Should()
                .BeEqual("timeFrame");
        }

        [Fact]
        public void Camelcase_Edges()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelcaseLabels()
                .EdgesModel
                .Labels
                .TryGetValue(typeof(LivesIn))
                .Should()
                .BeEqual("livesIn");
        }

        [Fact]
        public void Camelcase_Identifier_By_MemberExpression()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelcaseProperties()
                .GetIdentifier(Expression.Property(Expression.Constant(default, typeof(Person)), nameof(Person.RegistrationDate)))
                .Should()
                .Be("registrationDate");
        }

        [Fact]
        public void Camelcase_Identifier_By_ParameterExpression()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelcaseProperties()
                .GetIdentifier(Expression.Property(Expression.Constant(default, typeof(Person)), nameof(Person.RegistrationDate)))
                .Should()
                .Be("registrationDate");
        }

        [Fact]
        public void Camelcase_Mixed_Mode_Label()
        {
            var model = GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelcaseProperties();

            model
                .VerticesModel
                .Labels
                .TryGetValue(typeof(TimeFrame))
                .Should()
                .BeEqual("TimeFrame");

            model
                .GetIdentifier(Expression.Property(Expression.Constant(default, typeof(Person)), nameof(Person.RegistrationDate)))
                .Should()
                .Be("registrationDate");
        }

        [Fact]
        public void Camelcase_Mixed_Mode_Identifier()
        {
            var model = GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelcaseLabels();

            model
                .VerticesModel
                .Labels
                .TryGetValue(typeof(TimeFrame))
                .Should()
                .BeEqual("timeFrame");

            model
                .GetIdentifier(Expression.Property(Expression.Constant(default, typeof(Person)), nameof(Person.RegistrationDate)))
                .Should()
                .Be("RegistrationDate");
        }

        [Fact]
        public void Camelcase_Mixed_Mode_Combined()
        {
            var model = GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelcaseLabels()
                .WithCamelcaseProperties();

            model
                .VerticesModel
                .Labels
                .TryGetValue(typeof(TimeFrame))
                .Should()
                .BeEqual("timeFrame");

            model
                .GetIdentifier(Expression.Property(Expression.Constant(default, typeof(Person)), nameof(Person.RegistrationDate)))
                .Should()
                .Be("registrationDate");
        }

        [Fact]
        public void Camelcase_Mixed_Mode_Combined_Reversed()
        {
            var model = GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelcaseProperties()
                .WithCamelcaseLabels();

            model
                .VerticesModel
                .Labels
                .TryGetValue(typeof(TimeFrame))
                .Should()
                .BeEqual("timeFrame");

            model
                .GetIdentifier(Expression.Property(Expression.Constant(default, typeof(Person)), nameof(Person.RegistrationDate)))
                .Should()
                .Be("registrationDate");
        }

        [Fact]
        public void Configuration_IgnoreOnUpdate()
        {
            var metadata = GraphModel.FromBaseTypes<Vertex, Edge>()
                .ConfigureElement<Person>(builder =>
                {
                    builder.IgnoreOnUpdate(p => p.Name);
                })
                .GetPropertyMetadata(typeof(Person).GetProperty(nameof(Person.Name)));

            Assert.NotNull(metadata);
            Assert.Equal(IgnoreDirective.OnUpdate, metadata.IgnoreDirective);
        }

        [Fact]
        public void Configuration_IgnoreAlways()
        {
            var metadata = GraphModel.FromBaseTypes<Vertex, Edge>()
                .ConfigureElement<Person>(builder =>
                {
                    builder.IgnoreAlways(p => p.Name);
                })
                .GetPropertyMetadata(typeof(Person).GetProperty(nameof(Person.Name)));

            Assert.NotNull(metadata);
            Assert.Equal(IgnoreDirective.Always, metadata.IgnoreDirective);
        }

        [Fact]
        public void Configuration_Unconfigured()
        {
            var metadata = GraphModel.FromBaseTypes<Vertex, Edge>()
                .GetPropertyMetadata(typeof(Person).GetProperty(nameof(Person.Name)));

            Assert.NotNull(metadata);
            Assert.Equal(IgnoreDirective.Never, metadata.IgnoreDirective);
        }

        [Fact]
        public void Configuration_Before_Model_Changes()
        {
            var model = GraphModel.FromBaseTypes<Vertex, Edge>()
                .ConfigureElement<Person>(builder =>
                {
                    builder.IgnoreAlways(p => p.Name);
                })
                .WithCamelcaseLabels()
                .WithCamelcaseProperties();

            model
                .VerticesModel
                .Labels
                .TryGetValue(typeof(TimeFrame))
                .Should()
                .BeEqual("timeFrame");

            model
                .GetIdentifier(Expression.Property(Expression.Constant(default, typeof(Person)), nameof(Person.RegistrationDate)))
                .Should()
                .Be("registrationDate");

            var metadata = model.GetPropertyMetadata(typeof(Person).GetProperty(nameof(Person.Name)));

            Assert.NotNull(metadata);
            Assert.Equal(IgnoreDirective.Always, metadata.IgnoreDirective);    
        }

        [Fact]
        public void Configuration_After_Model_Changes()
        {
            var model = GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelcaseProperties()
                .WithCamelcaseLabels()
                .ConfigureElement<Person>(builder =>
                {
                    builder.IgnoreAlways(p => p.Name);
                });

            model
                .VerticesModel
                .Labels
                .TryGetValue(typeof(TimeFrame))
                .Should()
                .BeEqual("timeFrame");

            model
                .GetIdentifier(Expression.Property(Expression.Constant(default, typeof(Person)), nameof(Person.RegistrationDate)))
                .Should()
                .Be("registrationDate");

            var metadata = model.GetPropertyMetadata(typeof(Person).GetProperty(nameof(Person.Name)));

            Assert.NotNull(metadata);
            Assert.Equal(IgnoreDirective.Always, metadata.IgnoreDirective);
        }
    }
}
