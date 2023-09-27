using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GraphModelTest : VerifyBase
    {


        public GraphModelTest() : base()
        {

        }

        [ReleaseOnlyFact]
        public void Fody_works()
        {
            g
                .Invoking(_ => _
                    .ConfigureEnvironment(null!))
                .Should()
                .ThrowExactly<ArgumentNullException>()
                .WithMessage("[NullGuard] transformation is null. (Parameter 'transformation')");
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

        [ReleaseOnlyFact]
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
            var model = GraphModel.FromBaseTypes<Vertex, Edge>();

            await Verify(model.VerticesModel
                .TryGetFilterLabels(typeof(Authority), FilterLabelsVerbosity.Maximum) ?? ImmutableArray<string>.Empty);
        }

        [Fact]
        public async Task Hierarchy_inside_model()
        {
            await Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .VerticesModel
                .GetMetadata(typeof(Person)));
        }

        [Fact]
        public async Task Hierarchy_outside_model()
        {
            await Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .VerticesModel
                .GetMetadata(typeof(VertexInsideHierarchy)));
        }

        [Fact]
        public void Outside_hierarchy()
        {
            GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .VerticesModel
                .Invoking(_ => _
                    .GetMetadata(typeof(VertexOutsideHierarchy)))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public async Task Lowercase()
        {
            await Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .ConfigureElements(em => em
                    .UseLowerCaseLabels())
                .VerticesModel
                .GetMetadata(typeof(Person)));
        }

        [Fact]
        public async Task CamelcaseLabel_Vertices()
        {
            await Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .ConfigureElements(em => em
                    .UseCamelCaseLabels())
                .VerticesModel
                .GetMetadata(typeof(TimeFrame)));
        }

        [Fact]
        public async Task Camelcase_Edges()
        {
            await Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .ConfigureElements(em => em
                    .UseCamelCaseLabels())
                .EdgesModel
                .GetMetadata(typeof(LivesIn)));
        }

        [Fact]
        public async Task Camelcase_Identifier_By_MemberExpression()
        {
            await Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .ConfigureElements(pm => pm
                    .UseCamelCaseMemberNames())
                .VerticesModel
                .GetMetadata(typeof(Person).GetProperty(nameof(Person.RegistrationDate))!));
        }

        [Fact]
        public async Task Lowercase_Identifier_By_ParameterExpression()
        {
            await Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .ConfigureElements(pm => pm
                    .UseLowerCaseMemberNames())
                .VerticesModel
                .GetMetadata(typeof(Person).GetProperty(nameof(Person.RegistrationDate))!));
        }

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
                    .GetMetadata(typeof(TimeFrame)),
                model
                    .VerticesModel
                    .GetMetadata(typeof(Person).GetProperty(nameof(Person.RegistrationDate))!)));
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
                    .GetMetadata(typeof(TimeFrame)),
                model
                    .VerticesModel
                    .GetMetadata(typeof(Person).GetProperty(nameof(Person.RegistrationDate))!)));
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
                    .GetMetadata(typeof(TimeFrame)),
                model
                    .VerticesModel
                    .GetMetadata(typeof(Person).GetProperty(nameof(Person.RegistrationDate))!)));
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
                    .GetMetadata(typeof(TimeFrame)),
                model
                    .VerticesModel
                    .GetMetadata(typeof(Person).GetProperty(nameof(Person.RegistrationDate))!)));
        }

        [Fact]
        public async Task Configuration_IgnoreOnUpdate()
        {
            await Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .ConfigureVertices(_ => _
                    .ConfigureElement<Person>(conf => conf
                        .IgnoreOnUpdate(p => p.Name)))
                .VerticesModel
                .GetMetadata(typeof(Person).GetProperty(nameof(Person.Name))!));
        }

        [Fact]
        public async Task Configuration_can_be_found_for_base_class()
        {
            await Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .ConfigureVertices(pm => pm
                    .ConfigureElement<Person>(conf => conf
                        .IgnoreOnUpdate(p => p.Name)))
                .VerticesModel
                .GetMetadata(typeof(Authority).GetProperty(nameof(Authority.Name))!));
        }

        [Fact]
        public async Task Configuration_can_be_found_for_derived_class()
        {
            await Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .ConfigureVertices(pm => pm
                    .ConfigureElement<Authority>(conf => conf
                        .IgnoreOnUpdate(p => p.Name)))
                .VerticesModel
                .GetMetadata(typeof(Person).GetProperty(nameof(Person.Name))!));
        }

        [Fact]
        public async Task Configuration_IgnoreAlways()
        {
            await Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .ConfigureVertices(pm => pm
                    .ConfigureElement<Person>(conf => conf
                        .IgnoreAlways(p => p.Name)))
                .VerticesModel
                .GetMetadata(typeof(Person).GetProperty(nameof(Person.Name))!));
        }

        [Fact]
        public async Task Configuration_IgnoreAlways_Id()
        {
            await Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .ConfigureVertices(pm => pm
                    .ConfigureElement<Vertex>(conf => conf
                        .IgnoreAlways(p => p.Id)))
                .VerticesModel
                .GetMetadata(typeof(Person).GetProperty(nameof(Person.Id))!));
        }

        [Fact]
        public async Task Configuration_Unconfigured()
        {
            await Verify(GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .VerticesModel
                .GetMetadata(typeof(Person).GetProperty(nameof(Person.Name))!));
        }

        [Fact]
        public async Task Configuration_Before_Model_Changes()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .ConfigureVertices(pm => pm
                    .ConfigureElement<Person>(conf => conf
                        .IgnoreAlways(p => p.Name))
                    .UseCamelCaseMemberNames())
                .ConfigureElements(em => em
                    .UseCamelCaseLabels());

            await Verify((
                model
                    .VerticesModel
                    .GetMetadata(typeof(TimeFrame)),
                model
                    .VerticesModel
                    .GetMetadata(typeof(Person).GetProperty(nameof(Person.RegistrationDate))!),
                model
                    .VerticesModel
                    .GetMetadata(typeof(Person).GetProperty(nameof(Person.Name))!)));
        }

        [Fact]
        public async Task Configuration_After_Model_Changes()
        {
            var model = GraphModel
                .FromBaseTypes<Vertex, Edge>()
                .ConfigureVertices(pm => pm
                    .UseCamelCaseMemberNames()
                    .UseCamelCaseLabels()
                    .ConfigureElement<Person>(conf => conf
                        .IgnoreAlways(p => p.Name)));

            await Verify((
                model
                    .VerticesModel
                    .GetMetadata(typeof(TimeFrame)),
                model
                    .VerticesModel
                    .GetMetadata(typeof(Person).GetProperty(nameof(Person.RegistrationDate))!),
                model
                    .VerticesModel
                    .GetMetadata(typeof(Person).GetProperty(nameof(Person.Name))!)));
        }
    }
}
