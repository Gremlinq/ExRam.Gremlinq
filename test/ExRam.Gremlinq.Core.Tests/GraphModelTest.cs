using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Xunit;
using VerifyXunit;

namespace ExRam.Gremlinq.Core.Tests
{
    [UsesVerify]
    public class GraphModelTest
    {
        private sealed class VertexOutsideHierarchy
        {
            public object? Id { get; set; }
        }

        private sealed class VertexInsideHierarchy : Vertex
        {
        }

        [Fact]
        public void MemberMetadata_name_cannot_be_null()
        {
            var m = default(MemberMetadata);

            m
                .Invoking(_ => _.Key)
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void ElementMetadata_name_cannot_be_null()
        {
            var m = default(ElementMetadata);

            m
                .Invoking(_ => _.Label)
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public async Task TryGetFilterLabels_does_not_include_abstract_type()
        {
            var model = GraphModel.Default(lookup => lookup
                .IncludeAssembliesFromAppDomain());

            await Verifier.Verify(model.VerticesModel
                .TryGetFilterLabels(typeof(Authority), FilterLabelsVerbosity.Maximum) ?? ImmutableArray<string>.Empty);
        }

        [Fact]
        public async Task Hierarchy_inside_model()
        {
            await Verifier.Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .VerticesModel
                .Metadata
                .GetValueOrDefault(typeof(Person)));
        }

        [Fact]
        public async Task Hierarchy_outside_model()
        {
            await Verifier.Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(VertexInsideHierarchy), out var _));
        }

        [Fact]
        public async Task Outside_hierarchy()
        {
            await Verifier.Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .VerticesModel
                .Metadata
                .TryGetValue(typeof(VertexOutsideHierarchy), out var _));
        }

        [Fact]
        public async Task Lowercase()
        {
            await Verifier.Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureElements(em => em
                    .UseLowerCaseLabels())
                .VerticesModel
                .Metadata
                .GetValueOrDefault(typeof(Person)));
        }

        [Fact]
        public async Task CamelcaseLabel_Vertices()
        {
            await Verifier.Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureElements(em => em
                    .UseCamelCaseLabels())
                .VerticesModel
                .Metadata
                .GetValueOrDefault(typeof(TimeFrame)));
        }

        [Fact]
        public async Task Camelcase_Edges()
        {
            await Verifier.Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureElements(em => em
                    .UseCamelCaseLabels())
                .EdgesModel
                .Metadata
                .GetValueOrDefault(typeof(LivesIn)));
        }

        [Fact]
        public async Task Camelcase_Identifier_By_MemberExpression()
        {
            await Verifier.Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .ConfigureMemberMetadata(m => m
                        .UseCamelCaseNames()))
                .PropertiesModel
                .MemberMetadata
                .GetValueOrDefault(typeof(Person).GetProperty(nameof(Person.RegistrationDate))!));
        }

        [Fact]
        public async Task Lowercase_Identifier_By_ParameterExpression()
        {
            await Verifier.Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .ConfigureMemberMetadata(m => m
                        .UseLowerCaseNames()))
                .PropertiesModel
                .MemberMetadata
                .GetValueOrDefault(typeof(Person).GetProperty(nameof(Person.RegistrationDate))!));
        }

        [Fact]
        public async Task Camelcase_Mixed_Mode_Label()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .ConfigureMemberMetadata(m => m
                        .UseCamelCaseNames()));

            await Verifier.Verify((
                model
                    .VerticesModel
                    .Metadata
                    .GetValueOrDefault(typeof(TimeFrame)),
                model
                    .PropertiesModel
                    .MemberMetadata
                    .GetValueOrDefault(typeof(Person).GetProperty(nameof(Person.RegistrationDate))!)));
        }

        [Fact]
        public async Task Camelcase_Mixed_Mode_Identifier()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureElements(pm => pm
                    .UseCamelCaseLabels());

            await Verifier.Verify((
                model
                    .VerticesModel
                    .Metadata
                    .GetValueOrDefault(typeof(TimeFrame)),
                model
                    .PropertiesModel
                    .MemberMetadata
                    .GetValueOrDefault(typeof(Person).GetProperty(nameof(Person.RegistrationDate))!)));
        }

        [Fact]
        public async Task Camelcase_Mixed_Mode_Combined()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureElements(pm => pm
                    .UseCamelCaseLabels())
                .ConfigureProperties(pm => pm
                    .ConfigureMemberMetadata(m => m
                        .UseCamelCaseNames()));

            await Verifier.Verify((
                model
                    .VerticesModel
                    .Metadata
                    .GetValueOrDefault(typeof(TimeFrame)),
                model
                    .PropertiesModel
                    .MemberMetadata
                    .GetValueOrDefault(typeof(Person).GetProperty(nameof(Person.RegistrationDate))!)));
        }

        [Fact]
        public async Task Camelcase_Mixed_Mode_Combined_Reversed()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .ConfigureMemberMetadata(m => m
                        .UseCamelCaseNames()))
                .ConfigureElements(em => em
                    .UseCamelCaseLabels());

            await Verifier.Verify((
                model
                    .VerticesModel
                    .Metadata
                    .GetValueOrDefault(typeof(TimeFrame)),
                model
                    .PropertiesModel
                    .MemberMetadata
                    .GetValueOrDefault(typeof(Person).GetProperty(nameof(Person.RegistrationDate))!)));
        }

        [Fact]
        public async Task Configuration_IgnoreOnUpdate()
        {
            await Verifier.Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .ConfigureElement<Person>(conf => conf
                        .IgnoreOnUpdate(p => p.Name)))
                .PropertiesModel
                .MemberMetadata
                .GetValueOrDefault(typeof(Person).GetProperty(nameof(Person.Name))!));
        }

        [Fact]
        public async Task Configuration_can_be_found_for_base_class()
        {
            await Verifier.Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .ConfigureElement<Person>(conf => conf
                        .IgnoreOnUpdate(p => p.Name)))
                .PropertiesModel
                .MemberMetadata
                .GetValueOrDefault(typeof(Authority).GetProperty(nameof(Authority.Name))!));
        }

        [Fact]
        public async Task Configuration_can_be_found_for_derived_class()
        {
            await Verifier.Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .ConfigureElement<Authority>(conf => conf
                        .IgnoreOnUpdate(p => p.Name)))
                .PropertiesModel
                .MemberMetadata
                .GetValueOrDefault(typeof(Person).GetProperty(nameof(Person.Name))!));
        }

        [Fact]
        public async Task Equivalent_configuration_does_not_add_entry()
        {
            var model1 = GraphModel
                .Empty
                .ConfigureProperties(pm => pm
                    .ConfigureElement<Authority>(conf => conf
                        .IgnoreOnUpdate(p => p.Name)));

            var model2 = model1
                .ConfigureProperties(pm => pm
                    .ConfigureElement<Person>(conf => conf
                        .IgnoreOnUpdate(p => p.Name)));

            await Verifier.Verify(model1.PropertiesModel.MemberMetadata.Count == model2.PropertiesModel.MemberMetadata.Count);
        }

        [Fact]
        public async Task Configuration_IgnoreAlways()
        {
            await Verifier.Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .ConfigureElement<Person>(conf => conf
                        .IgnoreAlways(p => p.Name)))
                .PropertiesModel
                .MemberMetadata
                .GetValueOrDefault(typeof(Person).GetProperty(nameof(Person.Name))!));
        }

        [Fact]
        public async Task Configuration_IgnoreAlways_Id()
        {
            await Verifier.Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .ConfigureElement<IVertex>(conf => conf
                        .IgnoreAlways(p => p.Id)))
                .PropertiesModel
                .MemberMetadata
                .GetValueOrDefault(typeof(Person).GetProperty(nameof(Person.Id))!));
        }

        [Fact]
        public async Task Configuration_Unconfigured()
        {
            await Verifier.Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .PropertiesModel
                .MemberMetadata
                .GetValueOrDefault(typeof(Person).GetProperty(nameof(Person.Name))!));
        }

        [Fact]
        public async Task Configuration_Before_Model_Changes()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .ConfigureElement<Person>(conf => conf
                        .IgnoreAlways(p => p.Name))
                    .ConfigureMemberMetadata(m => m
                        .UseCamelCaseNames()))
                .ConfigureElements(em => em
                    .UseCamelCaseLabels());

            await Verifier.Verify((
                model
                    .VerticesModel
                    .Metadata
                    .GetValueOrDefault(typeof(TimeFrame)),
                model
                    .PropertiesModel
                    .MemberMetadata
                    .GetValueOrDefault(typeof(Person).GetProperty(nameof(Person.RegistrationDate))!),
                model
                    .PropertiesModel
                    .MemberMetadata
                    .GetValueOrDefault(typeof(Person).GetProperty(nameof(Person.Name))!)));
        }

        [Fact]
        public async Task Configuration_After_Model_Changes()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())
                .ConfigureProperties(pm => pm
                    .ConfigureMemberMetadata(m => m
                        .UseCamelCaseNames())
                    .ConfigureElement<Person>(conf => conf
                        .IgnoreAlways(p => p.Name)))
                .ConfigureElements(em => em
                    .UseCamelCaseLabels());

            await Verifier.Verify((
                model
                    .VerticesModel
                    .Metadata
                    .GetValueOrDefault(typeof(TimeFrame)),
                model
                    .PropertiesModel
                    .MemberMetadata
                    .GetValueOrDefault(typeof(Person).GetProperty(nameof(Person.RegistrationDate))!),
                model
                    .PropertiesModel
                    .MemberMetadata
                    .GetValueOrDefault(typeof(Person).GetProperty(nameof(Person.Name))!)));
        }
    }
}
