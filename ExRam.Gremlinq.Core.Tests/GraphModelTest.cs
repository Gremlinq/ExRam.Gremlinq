using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Xunit;
using LanguageExt;

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
        public void Hierarchy_inside_model()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(Person))
                .Bind(x => x.LabelOverride)
                .Should()
                .BeNone();
        }

        [Fact]
        public void Hierarchy_outside_model()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(VertexInsideHierarchy))
                .Should()
                .BeNone();
        }

        [Fact]
        public void Outside_hierarchy()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(VertexOutsideHierarchy))
                .Should()
                .BeNone();
        }

        [Fact]
        public void Lowercase()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .ConfigureElements(_ => _
                    .WithLowerCaseLabels())
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(Person))
                .Bind(x => x.LabelOverride)
                .Should()
                .BeSome("person");
        }

        [Fact]
        public void CamelcaseLabel_Verticies()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .ConfigureElements(_ => _
                    .WithCamelCaseLabels())
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(TimeFrame))
                .Bind(x => x.LabelOverride)
                .Should()
                .BeEqual("timeFrame");
        }

        [Fact]
        public void Camelcase_Edges()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .ConfigureElements(_ => _
                    .WithCamelCaseLabels())
                .EdgesModel
                .Metadata
                .TryGetValue(typeof(LivesIn))
                .Bind(x => x.LabelOverride)
                .Should()
                .BeEqual("livesIn");
        }

        [Fact]
        public void Camelcase_Identifier_By_MemberExpression()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .ConfigureProperties(_ => _
                    .WithCamelCaseProperties())
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Bind(x => x.IdentifierOverride)
                .Should()
                .BeSome("registrationDate");
        }

        [Fact]
        public void Camelcase_Identifier_By_ParameterExpression()
        {
            GraphModel.FromBaseTypes<Vertex, Edge>()
                .ConfigureProperties(_ => _
                    .WithCamelCaseProperties())
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Bind(x => x.IdentifierOverride)
                .Should()
                .BeSome("registrationDate");
        }

        [Fact]
        public void Camelcase_Mixed_Mode_Label()
        {
            var model = GraphModel.FromBaseTypes<Vertex, Edge>()
                .ConfigureProperties(_ => _
                    .WithCamelCaseProperties());

            model
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(TimeFrame))
                .Bind(x => x.LabelOverride)
                .Should()
                .BeNone();

            model
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Bind(x => x.IdentifierOverride)
                .Should()
                .BeSome("registrationDate");
        }

        [Fact]
        public void Camelcase_Mixed_Mode_Identifier()
        {
            var model = GraphModel.FromBaseTypes<Vertex, Edge>()
                .ConfigureElements(_ => _
                    .WithCamelCaseLabels());

            model
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(TimeFrame))
                .Bind(x => x.LabelOverride)
                .Should()
                .BeEqual("timeFrame");

            model
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Bind(x => x.IdentifierOverride)
                .Should()
                .BeNone();
        }

        [Fact]
        public void Camelcase_Mixed_Mode_Combined()
        {
            var model = GraphModel.FromBaseTypes<Vertex, Edge>()
                .ConfigureElements(_ => _
                    .WithCamelCaseLabels())
                .ConfigureProperties(_ => _
                    .WithCamelCaseProperties());

            model
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(TimeFrame))
                .Bind(x => x.LabelOverride)
                .Should()
                .BeEqual("timeFrame");

            model
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Bind(x => x.IdentifierOverride)
                .Should()
                .BeSome("registrationDate");
        }

        [Fact]
        public void Camelcase_Mixed_Mode_Combined_Reversed()
        {
            var model = GraphModel.FromBaseTypes<Vertex, Edge>()
                .ConfigureProperties(_ => _
                    .WithCamelCaseProperties())
                .ConfigureElements(_ => _
                    .WithCamelCaseLabels());

            model
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(TimeFrame))
                .Bind(x => x.LabelOverride)
                .Should()
                .BeEqual("timeFrame");

            model
                .PropertiesModel
                .Metadata
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
                .ConfigureProperties(_ => _
                    .ConfigureElement<Person>(builder => builder
                        .IgnoreOnUpdate(p => p.Name)))
                .PropertiesModel
                .Metadata
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
                .ConfigureProperties(_ => _
                    .ConfigureElement<Person>(builder => builder
                        .IgnoreAlways(p => p.Name)))
                .PropertiesModel
                .Metadata
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
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.Name)));

            maybeMetadata.IsSome
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Configuration_Before_Model_Changes()
        {
            var model = GraphModel.FromBaseTypes<Vertex, Edge>()
                .ConfigureProperties(_ => _
                    .ConfigureElement<Person>(builder => builder
                        .IgnoreAlways(p => p.Name))
                    .WithCamelCaseProperties())
                .ConfigureElements(_ => _
                    .WithCamelCaseLabels());

            model
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(TimeFrame))
                .Bind(x => x.LabelOverride)
                .Should()
                .BeEqual("timeFrame");

            model
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Bind(x => x.IdentifierOverride)
                .Should()
                .BeSome("registrationDate");

            var maybeMetadata = model
                .PropertiesModel
                .Metadata
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
                .ConfigureProperties(_ => _
                    .WithCamelCaseProperties()
                    .ConfigureElement<Person>(builder => builder
                        .IgnoreAlways(p => p.Name)))
                .ConfigureElements(_ => _
                    .WithCamelCaseLabels());

            model
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(TimeFrame))
                .Bind(x => x.LabelOverride)
                .Should()
                .BeEqual("timeFrame");

            model
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Bind(x => x.IdentifierOverride)
                .Should()
                .BeSome("registrationDate");

            var maybeMetadata = model
                .PropertiesModel
                .Metadata
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
