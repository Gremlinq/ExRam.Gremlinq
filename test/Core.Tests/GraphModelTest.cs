using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GraphModelTest
    {
        [Fact]
        public void ConfigureEnvironment_may_not_be_passed_null() => g
            .Invoking(_ => _
                .ConfigureEnvironment(null!))
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'transformation')");

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
        public async Task GetFilterLabels_does_not_include_abstract_type()
        {
            var model = GraphModel.FromBaseTypes<Vertex, Edge>();

            await Verify(model.VerticesModel
                .GetFilterLabels(FilterTypesCache<AbstractVertex>.Types, FilterLabelsVerbosity.Maximum));
        }

        [Fact]
        public async Task Hierarchy_inside_model() => await Verify(GraphModel
            .FromBaseTypes<Vertex, Edge>()
            .VerticesModel
            .GetMetadata(typeof(RichVertex)));

        [Fact]
        public async Task Hierarchy_outside_model() => GraphModel
            .FromBaseTypes<Vertex, Edge>()
            .VerticesModel
            .TryGetMetadata(typeof(VertexInsideHierarchy))
            .Should()
            .BeNull();

        [Fact]
        public void Outside_hierarchy() => GraphModel
            .FromBaseTypes<Vertex, Edge>()
            .VerticesModel
            .Invoking(_ => _
                .GetMetadata(typeof(VertexOutsideHierarchy)))
            .Should()
            .Throw<ArgumentException>();

        [Fact]
        public async Task Lowercase() => await Verify(GraphModel
            .FromBaseTypes<Vertex, Edge>()
            .ConfigureElements(em => em
                .UseLowerCaseLabels())
            .VerticesModel
            .GetMetadata(typeof(RichVertex)));

        [Fact]
        public async Task CamelcaseLabel_Vertices() => await Verify(GraphModel
            .FromBaseTypes<Vertex, Edge>()
            .ConfigureElements(em => em
                .UseCamelCaseLabels())
            .VerticesModel
            .GetMetadata(typeof(ScalarVertex)));

        [Fact]
        public async Task Camelcase_Edges() => await Verify(GraphModel
            .FromBaseTypes<Vertex, Edge>()
            .ConfigureElements(em => em
                .UseCamelCaseLabels())
            .EdgesModel
            .GetMetadata(typeof(PropertyEdge)));

        [Fact]
        public async Task Camelcase_Identifier_By_MemberExpression() => await Verify(GraphModel
            .FromBaseTypes<Vertex, Edge>()
            .ConfigureElements(pm => pm
                .UseCamelCaseMemberNames())
            .VerticesModel
            .GetMetadata(typeof(RichVertex).GetProperty(nameof(RichVertex.RegistrationDate))!));

        [Fact]
        public async Task Lowercase_Identifier_By_ParameterExpression() => await Verify(GraphModel
            .FromBaseTypes<Vertex, Edge>()
            .ConfigureElements(pm => pm
                .UseLowerCaseMemberNames())
            .VerticesModel
            .GetMetadata(typeof(RichVertex).GetProperty(nameof(RichVertex.RegistrationDate))!));

        [Fact]
        public async Task Camelcase_Mixed_Mode_Label()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .ConfigureElements(pm => pm
                    .UseCamelCaseMemberNames());

            await Verify((
                model
                    .VerticesModel
                    .GetMetadata(typeof(ScalarVertex)),
                model
                    .VerticesModel
                    .GetMetadata(typeof(RichVertex).GetProperty(nameof(RichVertex.RegistrationDate))!)));
        }

        [Fact]
        public async Task Camelcase_Mixed_Mode_Identifier()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .ConfigureElements(pm => pm
                    .UseCamelCaseLabels());

            await Verify((
                model
                    .VerticesModel
                    .GetMetadata(typeof(ScalarVertex)),
                model
                    .VerticesModel
                    .GetMetadata(typeof(RichVertex).GetProperty(nameof(RichVertex.RegistrationDate))!)));
        }

        [Fact]
        public async Task Camelcase_Mixed_Mode_Combined()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .ConfigureElements(pm => pm
                    .UseCamelCaseLabels()
                    .UseCamelCaseMemberNames());

            await Verify((
                model
                    .VerticesModel
                    .GetMetadata(typeof(ScalarVertex)),
                model
                    .VerticesModel
                    .GetMetadata(typeof(RichVertex).GetProperty(nameof(RichVertex.RegistrationDate))!)));
        }

        [Fact]
        public async Task Camelcase_Mixed_Mode_Combined_Reversed()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .ConfigureElements(pm => pm
                    .UseCamelCaseMemberNames()
                    .UseCamelCaseLabels());

            await Verify((
                model
                    .VerticesModel
                    .GetMetadata(typeof(ScalarVertex)),
                model
                    .VerticesModel
                    .GetMetadata(typeof(RichVertex).GetProperty(nameof(RichVertex.RegistrationDate))!)));
        }

        [Fact]
        public async Task Configuration_IgnoreOnUpdate() => await Verify(GraphModel
            .FromBaseTypes<Vertex, Edge>()
            .ConfigureVertices(_ => _
                .ConfigureElement<RichVertex>(conf => conf
                    .IgnoreOnUpdate(p => p.Name)))
            .VerticesModel
            .GetMetadata(typeof(RichVertex).GetProperty(nameof(RichVertex.Name))!));

        [Fact]
        public async Task Configuration_can_be_found_for_base_class() => await Verify(GraphModel
            .FromBaseTypes<Vertex, Edge>()
            .ConfigureVertices(pm => pm
                .ConfigureElement<RichVertex>(conf => conf
                    .IgnoreOnUpdate(p => p.Name)))
            .VerticesModel
            .GetMetadata(typeof(AbstractVertex).GetProperty(nameof(AbstractVertex.Name))!));

        [Fact]
        public async Task Configuration_can_be_found_for_derived_class() => await Verify(GraphModel
            .FromBaseTypes<Vertex, Edge>()
            .ConfigureVertices(pm => pm
                .ConfigureElement<AbstractVertex>(conf => conf
                    .IgnoreOnUpdate(p => p.Name)))
            .VerticesModel
            .GetMetadata(typeof(RichVertex).GetProperty(nameof(RichVertex.Name))!));

        [Fact]
        public async Task Configuration_IgnoreAlways() => await Verify(GraphModel
            .FromBaseTypes<Vertex, Edge>()
            .ConfigureVertices(pm => pm
                .ConfigureElement<RichVertex>(conf => conf
                    .IgnoreAlways(p => p.Name)))
            .VerticesModel
            .GetMetadata(typeof(RichVertex).GetProperty(nameof(RichVertex.Name))!));

        [Fact]
        public async Task Configuration_IgnoreAlways_Id() => await Verify(GraphModel
            .FromBaseTypes<Vertex, Edge>()
            .ConfigureVertices(pm => pm
                .ConfigureElement<Vertex>(conf => conf
                    .IgnoreAlways(p => p.Id)))
            .VerticesModel
            .GetMetadata(typeof(RichVertex).GetProperty(nameof(RichVertex.Id))!));

        [Fact]
        public async Task Configuration_Unconfigured() => await Verify(GraphModel
            .FromBaseTypes<Vertex, Edge>()
            .VerticesModel
            .GetMetadata(typeof(RichVertex).GetProperty(nameof(RichVertex.Name))!));

        [Fact]
        public async Task Configuration_Before_Model_Changes()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .ConfigureVertices(pm => pm
                    .ConfigureElement<RichVertex>(conf => conf
                        .IgnoreAlways(p => p.Name))
                    .UseCamelCaseMemberNames())
                .ConfigureElements(em => em
                    .UseCamelCaseLabels());

            await Verify((
                model
                    .VerticesModel
                    .GetMetadata(typeof(ScalarVertex)),
                model
                    .VerticesModel
                    .GetMetadata(typeof(RichVertex).GetProperty(nameof(RichVertex.RegistrationDate))!),
                model
                    .VerticesModel
                    .GetMetadata(typeof(RichVertex).GetProperty(nameof(RichVertex.Name))!)));
        }

        [Fact]
        public async Task Configuration_After_Model_Changes()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .ConfigureVertices(pm => pm
                    .UseCamelCaseMemberNames()
                    .UseCamelCaseLabels()
                    .ConfigureElement<RichVertex>(conf => conf
                        .IgnoreAlways(p => p.Name)));

            await Verify((
                model
                    .VerticesModel
                    .GetMetadata(typeof(ScalarVertex)),
                model
                    .VerticesModel
                    .GetMetadata(typeof(RichVertex).GetProperty(nameof(RichVertex.RegistrationDate))!),
                model
                    .VerticesModel
                    .GetMetadata(typeof(RichVertex).GetProperty(nameof(RichVertex.Name))!)));
        }

        [Fact]
        public async Task Elements_outside_of_assemblies()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>();

            model
                .VerticesModel
                .TryGetMetadata(typeof(VertexInsideHierarchy))
                .Should()
                .BeNull();

            model
                .VerticesModel
                .TryGetMetadata(typeof(VertexInsideHierarchy).GetProperty(nameof(VertexInsideHierarchy.ExtraProperty))!)
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task AddAssemblies()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .AddAssemblies(typeof(VertexInsideHierarchy).Assembly);

            model
                .VerticesModel
                .TryGetMetadata(typeof(VertexInsideHierarchy))
                .Should()
                .NotBeNull();

            model
                .VerticesModel
                .TryGetMetadata(typeof(VertexInsideHierarchy).GetProperty(nameof(VertexInsideHierarchy.ExtraProperty))!)
                .Should()
                .NotBeNull();
        }

        [Fact]
        public async Task Configuration_IgnoreAlways_Id_when_inheriting_from_Element() => await Verify(GraphModel
            .FromBaseTypes<VertexElement, EdgeElement>()
            .ConfigureVertices(pm => pm
                .ConfigureElement<VertexElement>(conf => conf
                    .IgnoreAlways(p => p.Id)))
            .VerticesModel
            .GetMetadata(typeof(VertexElement).GetProperty(nameof(VertexElement.Id))!));

        [Fact]
        public async Task Non_abstract_base_types_are_included()
        {
            var a = GraphModel
                .FromBaseTypes<NonAbstractBaseVertex, NonAbstractBaseEdge>()
                .VerticesModel
                .GetFilterLabels(FilterTypesCache<NonAbstractBaseVertex>.Types, FilterLabelsVerbosity.Maximum);

            await Verify(GraphModel
                .FromBaseTypes<NonAbstractBaseVertex, NonAbstractBaseEdge>()
                .VerticesModel
                .GetFilterLabels(FilterTypesCache<NonAbstractBaseVertex>.Types, FilterLabelsVerbosity.Maximum));
        }
    }
}
