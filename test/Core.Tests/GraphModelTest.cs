using System.Reflection;
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
        public void MemberMetadata_does_not_equal_an_object_of_another_type() => MemberMetadata
            .Default("name")
            .Equals((object)"name")
            .Should()
            .BeFalse();

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

        [Fact]
        public void FromBaseTypes_rejects_an_edge_base_type_derived_from_the_vertex_base_type() => new Action(() => GraphModel
            .FromBaseTypes<Vertex, RichVertex>())
            .Should()
            .ThrowExactly<ArgumentException>()
            .WithMessage($"The vertex base type {typeof(Vertex)} may not be in the inheritance hierarchy of the edge base type {typeof(RichVertex)}.");

        [Fact]
        public void FromBaseTypes_rejects_a_vertex_base_type_derived_from_the_edge_base_type() => new Action(() => GraphModel
            .FromBaseTypes<RichVertex, Vertex>())
            .Should()
            .ThrowExactly<ArgumentException>()
            .WithMessage($"The edge base type {typeof(Vertex)} may not be in the inheritance hierarchy of the vertex base type {typeof(RichVertex)}.");

        [Fact]
        public void Invalid_model_throws_on_every_member()
        {
            var model = GraphModel.Invalid.VerticesModel;
            var member = typeof(Vertex).GetProperty(nameof(Vertex.Id))!;

            model.Invoking(_ => _.ElementTypes).Should().ThrowExactly<InvalidOperationException>().WithMessage("ElementTypes must not be called on GraphElementModel.Invalid.*");
            model.Invoking(_ => _.Members).Should().ThrowExactly<InvalidOperationException>().WithMessage("Members must not be called on GraphElementModel.Invalid.*");
            model.Invoking(_ => _.AddAssemblies()).Should().ThrowExactly<InvalidOperationException>().WithMessage("AddAssemblies must not be called on GraphElementModel.Invalid.*");
            model.Invoking(_ => _.ConfigureMetadata(member, static metadata => metadata)).Should().ThrowExactly<InvalidOperationException>().WithMessage("ConfigureMetadata must not be called on GraphElementModel.Invalid.*");
            model.Invoking(_ => _.ConfigureMetadata(static (MemberInfo _, MemberMetadata metadata) => metadata)).Should().ThrowExactly<InvalidOperationException>().WithMessage("ConfigureMetadata must not be called on GraphElementModel.Invalid.*");
            model.Invoking(_ => _.ConfigureMetadata(typeof(Vertex), static metadata => metadata)).Should().ThrowExactly<InvalidOperationException>().WithMessage("ConfigureMetadata must not be called on GraphElementModel.Invalid.*");
            model.Invoking(_ => _.ConfigureMetadata(static (Type _, ElementMetadata metadata) => metadata)).Should().ThrowExactly<InvalidOperationException>().WithMessage("ConfigureMetadata must not be called on GraphElementModel.Invalid.*");
            model.Invoking(_ => _.TryGetMetadata(typeof(Vertex))).Should().ThrowExactly<InvalidOperationException>().WithMessage("TryGetMetadata must not be called on GraphElementModel.Invalid.*");
            model.Invoking(_ => _.TryGetMetadata(member)).Should().ThrowExactly<InvalidOperationException>().WithMessage("TryGetMetadata must not be called on GraphElementModel.Invalid.*");
        }

        [Fact]
        public void ConfigureMetadata_of_an_element_type() => GraphModel
            .FromBaseTypes<Vertex, Edge>()
            .VerticesModel
            .ConfigureMetadata(typeof(RichVertex), static _ => new ElementMetadata("Rich"))
            .GetMetadata(typeof(RichVertex))
            .Label
            .Should()
            .Be("Rich");

        [Fact]
        public void ConfigureMetadata_of_a_type_outside_the_model() => GraphModel
            .FromBaseTypes<Vertex, Edge>()
            .VerticesModel
            .Invoking(_ => _.ConfigureMetadata(typeof(string), static metadata => metadata))
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage($"The type {typeof(string).FullName} is outside of the model.");

        [Fact]
        public void ResetSerializationBehaviour_undoes_IgnoreAlways() => GraphModel
            .FromBaseTypes<Vertex, Edge>()
            .ConfigureVertices(pm => pm
                .ConfigureElement<RichVertex>(conf => conf
                    .IgnoreAlways(p => p.Name)
                    .ResetSerializationBehaviour(p => p.Name)))
            .VerticesModel
            .GetMetadata(typeof(RichVertex).GetProperty(nameof(RichVertex.Name))!)
            .SerializationBehaviour
            .Should()
            .Be(SerializationBehaviour.Default);

        [Fact]
        public void Equal_MemberMetadata_have_equal_hash_codes() => new MemberMetadata("name", SerializationBehaviour.IgnoreAlways)
            .GetHashCode()
            .Should()
            .Be(new MemberMetadata("name", SerializationBehaviour.IgnoreAlways).GetHashCode());

        [Fact]
        public void Equal_ElementMetadata_have_equal_hash_codes() => new ElementMetadata("label")
            .GetHashCode()
            .Should()
            .Be(new ElementMetadata("label").GetHashCode());
    }
}
