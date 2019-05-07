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
                .WithLowerCaseLabels()
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
                .WithCamelCaseLabels()
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
                .WithCamelCaseLabels()
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
                .WithCamelCaseProperties()
                .PropertiesModel
                .MetaData
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Bind(x => x.IdentifierOverride)
                .Should()
                .BeSome("registrationDate");
        }

        [Fact]
        public void Camelcase_Identifier_By_ParameterExpression()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelCaseProperties()
                .PropertiesModel
                .MetaData
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Bind(x => x.IdentifierOverride)
                .Should()
                .BeSome("registrationDate");
        }

        [Fact]
        public void Camelcase_Mixed_Mode_Label()
        {
            var model = GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelCaseProperties();

            model
                .VerticesModel
                .Labels
                .TryGetValue(typeof(TimeFrame))
                .Should()
                .BeEqual("TimeFrame");

            model
                .PropertiesModel
                .MetaData
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Bind(x => x.IdentifierOverride)
                .Should()
                .BeSome("registrationDate");
        }

        [Fact]
        public void Camelcase_Mixed_Mode_Identifier()
        {
            var model = GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelCaseLabels();

            model
                .VerticesModel
                .Labels
                .TryGetValue(typeof(TimeFrame))
                .Should()
                .BeEqual("timeFrame");

            model
                .PropertiesModel
                .MetaData
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Bind(x => x.IdentifierOverride)
                .Should()
                .BeNone();
        }

        [Fact]
        public void Camelcase_Mixed_Mode_Combined()
        {
            var model = GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelCaseLabels()
                .WithCamelCaseProperties();

            model
                .VerticesModel
                .Labels
                .TryGetValue(typeof(TimeFrame))
                .Should()
                .BeEqual("timeFrame");

            model
                .PropertiesModel
                .MetaData
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Bind(x => x.IdentifierOverride)
                .Should()
                .BeSome("registrationDate");
        }

        [Fact]
        public void Camelcase_Mixed_Mode_Combined_Reversed()
        {
            var model = GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelCaseProperties()
                .WithCamelCaseLabels();

            model
                .VerticesModel
                .Labels
                .TryGetValue(typeof(TimeFrame))
                .Should()
                .BeEqual("timeFrame");

            model
                .PropertiesModel
                .MetaData
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Bind(x => x.IdentifierOverride)
                .Should()
                .BeSome("registrationDate");
        }

        [Fact]
        public void Configuration_IgnoreOnUpdate()
        {
            var maybeMetadata = GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .ConfigureElement<Person>(builder =>
                {
                    builder.IgnoreOnUpdate(p => p.Name);
                })
                .PropertiesModel
                .MetaData
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.Name)));

            maybeMetadata
                .Should()
                .BeSome(metaData => metaData
                    .IgnoreDirective
                    .Should()
                    .Be(SerializationDirective.IgnoreOnUpdate));
        }

        [Fact]
        public void Configuration_IgnoreAlways()
        {
            var maybeMetadata = GraphModel.FromBaseTypes<Vertex, Edge>()
                .ConfigureElement<Person>(builder =>
                {
                    builder.IgnoreAlways(p => p.Name);
                })
                .PropertiesModel
                .MetaData
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.Name)));

            maybeMetadata
                .Should()
                .BeSome(metaData => metaData
                    .IgnoreDirective
                    .Should()
                    .Be(SerializationDirective.IgnoreAlways));
        }

        [Fact]
        public void Configuration_Unconfigured()
        {
            var maybeMetadata = GraphModel.FromBaseTypes<Vertex, Edge>()
                .PropertiesModel
                .MetaData
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.Name)));

            maybeMetadata.IsSome
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Configuration_Before_Model_Changes()
        {
            var model = GraphModel.FromBaseTypes<Vertex, Edge>()
                .ConfigureElement<Person>(builder =>
                {
                    builder.IgnoreAlways(p => p.Name);
                })
                .WithCamelCaseLabels()
                .WithCamelCaseProperties();

            model
                .VerticesModel
                .Labels
                .TryGetValue(typeof(TimeFrame))
                .Should()
                .BeEqual("timeFrame");

            model
                .PropertiesModel
                .MetaData
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Bind(x => x.IdentifierOverride)
                .Should()
                .BeSome("registrationDate");

            var maybeMetadata = model
                .PropertiesModel
                .MetaData
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.Name)));

            maybeMetadata
                .Should()
                .BeSome(metaData => metaData
                    .IgnoreDirective
                    .Should()
                    .Be(SerializationDirective.IgnoreAlways));
        }

        [Fact]
        public void Configuration_After_Model_Changes()
        {
            var model = GraphModel.FromBaseTypes<Vertex, Edge>()
                .WithCamelCaseProperties()
                .WithCamelCaseLabels()
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
                .PropertiesModel
                .MetaData
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Bind(x => x.IdentifierOverride)
                .Should()
                .BeSome("registrationDate");

            var maybeMetadata = model
                .PropertiesModel
                .MetaData
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.Name)));

            maybeMetadata
                .Should()
                .BeSome(metaData => metaData
                    .IgnoreDirective
                    .Should()
                    .Be(SerializationDirective.IgnoreAlways));
        }
    }
}
