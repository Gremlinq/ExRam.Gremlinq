using System;
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
        public void PropertyMetadata_name_cannot_be_null()
        {
            var m = default(PropertyMetadata);

            m
                .Invoking(_ => _.Name)
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void TryGetFilterLabels_does_not_include_abstract_type()
        {
            var model = GraphModel.Default(lookup => lookup
                .IncludeAssembliesFromAppDomain());

            (model.VerticesModel
                .TryGetFilterLabels(typeof(Authority), FilterLabelsVerbosity.Maximum) ?? Array.Empty<string>())
                .Should()
                .Contain("Company").And
                .Contain("Person").And
                .NotContain("Authority");
        }

        [Fact]
        public void Hierarchy_inside_model()
        {
            GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(Person))
                .Map(x => x.Label)
                .Should()
                .BeSome("Person");
        }

        [Fact]
        public void Hierarchy_outside_model()
        {
            GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(VertexInsideHierarchy))
                .Should()
                .BeNone();
        }

        [Fact]
        public void Outside_hierarchy()
        {
            GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(VertexOutsideHierarchy))
                .Should()
                .BeNone();
        }

        [Fact]
        public void Lowercase()
        {
            GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureElements(em => em
                    .UseLowerCaseLabels())
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(Person))
                .Map(x => x.Label)
                .Should()
                .BeSome("person");
        }

        [Fact]
        public void CamelcaseLabel_Vertices()
        {
            GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureElements(em => em
                    .UseCamelCaseLabels())
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(TimeFrame))
                .Map(x => x.Label)
                .Should()
                .BeEqual("timeFrame");
        }

        [Fact]
        public void Camelcase_Edges()
        {
            GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureElements(em => em
                    .UseCamelCaseLabels())
                .EdgesModel
                .Metadata
                .TryGetValue(typeof(LivesIn))
                .Map(x => x.Label)
                .Should()
                .BeEqual("livesIn");
        }

        [Fact]
        public void Camelcase_Identifier_By_MemberExpression()
        {
            GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .UseCamelCaseNames())
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Map(x => x.Name)
                .Should()
                .BeSome("registrationDate");
        }

        [Fact]
        public void Camelcase_Identifier_By_ParameterExpression()
        {
            GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .UseCamelCaseNames())
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Map(x => x.Name)
                .Should()
                .BeSome("registrationDate");
        }

        [Fact]
        public void Camelcase_Mixed_Mode_Label()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .UseCamelCaseNames());

            model
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(TimeFrame))
                .Map(x => x.Label)
                .Should()
                .BeSome("TimeFrame");

            model
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Map(x => x.Name)
                .Should()
                .BeSome("registrationDate");
        }

        [Fact]
        public void Camelcase_Mixed_Mode_Identifier()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureElements(pm => pm
                    .UseCamelCaseLabels());

            model
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(TimeFrame))
                .Map(x => x.Label)
                .Should()
                .BeEqual("timeFrame");

            model
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Map(x => x.Name)
                .Should()
                .BeEqual("RegistrationDate");
        }

        [Fact]
        public void Camelcase_Mixed_Mode_Combined()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureElements(pm => pm
                    .UseCamelCaseLabels())
                .ConfigureProperties(pm => pm
                    .UseCamelCaseNames());

            model
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(TimeFrame))
                .Map(x => x.Label)
                .Should()
                .BeEqual("timeFrame");

            model
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Map(x => x.Name)
                .Should()
                .BeSome("registrationDate");
        }

        [Fact]
        public void Camelcase_Mixed_Mode_Combined_Reversed()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .UseCamelCaseNames())
                .ConfigureElements(em => em
                    .UseCamelCaseLabels());

            model
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(TimeFrame))
                .Map(x => x.Label)
                .Should()
                .BeEqual("timeFrame");

            model
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Map(x => x.Name)
                .Should()
                .BeSome("registrationDate");
        }

        [Fact]
        public void Configuration_IgnoreOnUpdate()
        {
            GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .ConfigureElement<Person>(conf => conf
                        .IgnoreOnUpdate(p => p.Name)))
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.Name)))
                .Should()
                .BeSome(metaData => metaData
                    .SerializationBehaviour
                    .Should()
                    .Be(SerializationBehaviour.IgnoreOnUpdate));
        }

        [Fact]
        public void Configuration_can_be_found_for_base_class()
        {
            GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .ConfigureElement<Person>(conf => conf
                        .IgnoreOnUpdate(p => p.Name)))
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Authority).GetProperty(nameof(Authority.Name)))
                .Should()
                .BeSome(metaData => metaData
                    .SerializationBehaviour
                    .Should()
                    .Be(SerializationBehaviour.IgnoreOnUpdate));
        }

        [Fact]
        public void Configuration_can_be_found_for_derived_class()
        {
            GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .ConfigureElement<Authority>(conf => conf
                        .IgnoreOnUpdate(p => p.Name)))
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.Name)))
                .Should()
                .BeSome(metaData => metaData
                    .SerializationBehaviour
                    .Should()
                    .Be(SerializationBehaviour.IgnoreOnUpdate));
        }

        [Fact]
        public void Equivalent_configuration_does_not_add_entry()
        {
            var model = GraphModel
                .Empty
                .ConfigureProperties(pm => pm
                    .ConfigureElement<Authority>(conf => conf
                        .IgnoreOnUpdate(p => p.Name)));

            model.PropertiesModel.Metadata
                .Should()
                .HaveCount(1);

            model = model
                .ConfigureProperties(pm => pm
                    .ConfigureElement<Person>(conf => conf
                        .IgnoreOnUpdate(p => p.Name)));

            model.PropertiesModel.Metadata
                .Should()
                .HaveCount(1);
        }

        [Fact]
        public void Configuration_IgnoreAlways()
        {
            var maybeMetadata = GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .ConfigureElement<Person>(conf => conf
                        .IgnoreAlways(p => p.Name)))
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.Name)));

            maybeMetadata
                .Should()
                .BeSome(metaData => metaData
                    .SerializationBehaviour
                    .Should()
                    .Be(SerializationBehaviour.IgnoreAlways));
        }

        [Fact]
        public void Configuration_Unconfigured()
        {
            var maybeMetadata = GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
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
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .ConfigureElement<Person>(conf => conf
                        .IgnoreAlways(p => p.Name))
                    .UseCamelCaseNames())
                .ConfigureElements(em => em
                    .UseCamelCaseLabels());

            model
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(TimeFrame))
                .Map(x => x.Label)
                .Should()
                .BeEqual("timeFrame");

            model
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Map(x => x.Name)
                .Should()
                .BeSome("registrationDate");

            var maybeMetadata = model
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.Name)));

            maybeMetadata
                .Should()
                .BeSome(metaData => metaData
                    .SerializationBehaviour
                    .Should()
                    .Be(SerializationBehaviour.IgnoreAlways));
        }

        [Fact]
        public void Configuration_After_Model_Changes()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .UseCamelCaseNames()
                    .ConfigureElement<Person>(conf => conf
                        .IgnoreAlways(p => p.Name)))
                .ConfigureElements(em => em
                    .UseCamelCaseLabels());

            model
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(TimeFrame))
                .Map(x => x.Label)
                .Should()
                .BeEqual("timeFrame");

            model
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.RegistrationDate)))
                .Map(x => x.Name)
                .Should()
                .BeSome("registrationDate");

            var maybeMetadata = model
                .PropertiesModel
                .Metadata
                .TryGetValue(typeof(Person).GetProperty(nameof(Person.Name)));

            maybeMetadata
                .Should()
                .BeSome(metaData => metaData
                    .SerializationBehaviour
                    .Should()
                    .Be(SerializationBehaviour.IgnoreAlways));
        }
    }
}
