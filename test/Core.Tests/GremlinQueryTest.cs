using System.Collections.Immutable;

using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Gremlin.Net.Process.Traversal;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GremlinQueryTest
    {
        private readonly IGremlinQuerySource _g;

        public GremlinQueryTest()
        {
            _g = g
                .ConfigureEnvironment(_ => _
                    .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>()));
        }

        [Fact]
        public void ChangeQueryType_to_IGremlinQuerySource() => _g
            .V()
            .AsAdmin()
            .ChangeQueryType<IGremlinQuerySource>()
            .Should()
            .NotBeNull();

        [Fact]
        public void AsAdmin_AddStep() => _g
            .V()
            .AsAdmin()
            .AddStep<IGremlinQuerySource>(
                new InjectStep(ImmutableArray<object>.Empty.Add(0)),
                _ => _.Fold())
            .Should()
            .NotBeNull();

        [Fact]
        public void AsAdmin_AddSteps() => _g
            .V()
            .AsAdmin()
            .AddSteps<IGremlinQuery<object>>([new InjectStep(ImmutableArray<object>.Empty.Add(0)), FoldStep.Instance])
            .Should()
            .NotBeNull();

        [Fact]
        public Task AsAdmin_ConfigureMetadata() => Verify(_g
            .V()
            .AsAdmin()
            .ConfigureMetadata<IGremlinQuerySource>(dict => dict.SetItem("key", "value"))
            .AsAdmin()
            .Metadata);

        [Fact]
        public Task AsAdmin_ConfigureMetadata_On_GremlinQuerySource() => Verify(_g
                .AsAdmin()
                .ConfigureMetadata<IGremlinQuerySource>(dict => dict.SetItem("key", "value"))
                .AsAdmin()
                .Metadata);

        [Fact]
        public void ChangeQueryType_optimizes()
        {
            var query = _g
                .V<RichVertex>();

            query.AsAdmin().ChangeQueryType<IVertexGremlinQuery<RichVertex>>()
                .Should()
                .BeSameAs(query);

            query.AsAdmin().ChangeQueryType<IGremlinQuery<RichVertex>>()
                .Should()
                .BeSameAs(query);

            query.AsAdmin().ChangeQueryType<IGremlinQueryBase>()
                .Should()
                .BeSameAs(query);

            query.AsAdmin().ChangeQueryType<IVertexGremlinQuery<object>>()
                .Should()
                .NotBeSameAs(query);

            query.AsAdmin().ChangeQueryType<IEdgeGremlinQuery<object>>()
                .Should()
                .NotBeSameAs(query);
        }

        [Fact]
        public void ChangeQueryType_takes_array_element_types_into_account()
        {
            var query = _g
                .V<RichVertex>();

            query.AsAdmin().ChangeQueryType<IGremlinQueryBase<RichVertex[]>>()
                .Should()
                .BeAssignableTo<IArrayGremlinQueryBase<RichVertex>>();
        }

        [Fact]
        public async Task ForceVertex_has_correct_semantics() => await Verify(_g
            .V<RichVertex>()
            .Count()
            .ForceVertex()
            .ToTraversal()
            .Projection);

        [Fact]
        public void None_adds_the_step_of_Discard()
        {
#pragma warning disable CS0618 // None() stays until the next major version and must behave like Discard().
            var steps = _g
                .V()
                .None()
                .AsAdmin()
                .Steps;
#pragma warning restore CS0618

            steps[^1]
                .Should()
                .BeSameAs(_g
                    .V()
                    .Discard()
                    .AsAdmin()
                    .Steps[^1]);
        }

        [Fact]
        public void Discard_adds_NoneStep_below_TinkerPop_3_8() => _g
            .V()
            .Discard()
            .AsAdmin()
            .Steps[^1]
            .Should()
            .BeSameAs(NoneStep.Instance);

        [Fact]
        public void Discard_adds_DiscardStep_from_TinkerPop_3_8() => _g
            .ConfigureEnvironment(environment => environment
                .ConfigureServerCompatibilityRange(_ => VersionRange.From(TinkerPopVersions.V3_8_2, TinkerPopVersions.V3_8_2)))
            .V()
            .Discard()
            .AsAdmin()
            .Steps[^1]
            .Should()
            .BeSameAs(DiscardStep.Instance);

        [Fact]
        public void Disjoint_labels_serialize_as_discard_from_TinkerPop_3_8()
        {
            var query = _g
                .ConfigureEnvironment(environment => environment
                    .ConfigureServerCompatibilityRange(_ => VersionRange.From(TinkerPopVersions.V3_8_2, TinkerPopVersions.V3_8_2)))
                .V()
                .AsAdmin()
                .AddStep<IVertexGremlinQuery<object>>(new HasLabelStep(["Person"]))
                .AsAdmin()
                .AddStep<IVertexGremlinQuery<object>>(new HasLabelStep(["Company"]));

            var environment = query.AsAdmin().Environment;

            environment.Serializer
                .TransformTo<Bytecode>()
                .From(query, environment)
                .StepInstructions
                .Select(instruction => instruction.OperatorName)
                .Should()
                .Contain("discard")
                .And
                .NotContain("hasLabel")
                .And
                .NotContain("none");
        }

        [Fact]
        public void Where_on_the_id_adds_HasTraversalStep_below_TinkerPop_3_8() => _g
            .V<RichVertex>()
            .Where(x => x.Id, _ => _.Fold())
            .AsAdmin()
            .Steps[^1]
            .Should()
            .BeOfType<HasTraversalStep>()
            .Which
            .Key
            .Should()
            .Be(new Key(T.Id));

        [Fact]
        public void Where_on_the_id_filters_by_id_from_TinkerPop_3_8() => _g
            .ConfigureEnvironment(environment => environment
                .ConfigureServerCompatibilityRange(_ => VersionRange.From(TinkerPopVersions.V3_8_2, TinkerPopVersions.V3_8_2)))
            .V<RichVertex>()
            .Where(x => x.Id, _ => _.Fold())
            .AsAdmin()
            .Steps[^1]
            .Should()
            .BeOfType<WhereTraversalStep>()
            .Which
            .Traversal
            .Steps
            .ToArray()
            .Should()
            .Equal(IdStep.Instance, FoldStep.Instance);

        [Fact]
        public void Where_on_the_label_adds_HasTraversalStep_below_TinkerPop_3_8() => _g
            .V<RichVertex>()
            .Where(x => x.Label, _ => _.Fold())
            .AsAdmin()
            .Steps[^1]
            .Should()
            .BeOfType<HasTraversalStep>()
            .Which
            .Key
            .Should()
            .Be(new Key(T.Label));

        [Fact]
        public void Where_on_the_label_filters_by_label_from_TinkerPop_3_8() => _g
            .ConfigureEnvironment(environment => environment
                .ConfigureServerCompatibilityRange(_ => VersionRange.From(TinkerPopVersions.V3_8_2, TinkerPopVersions.V3_8_2)))
            .V<RichVertex>()
            .Where(x => x.Label, _ => _.Fold())
            .AsAdmin()
            .Steps[^1]
            .Should()
            .BeOfType<WhereTraversalStep>()
            .Which
            .Traversal
            .Steps
            .ToArray()
            .Should()
            .Equal(LabelStep.Instance, FoldStep.Instance);

        [Fact]
        public void Where_on_a_member_with_key_T_key_filters_by_its_values_from_TinkerPop_3_8() => _g
            .ConfigureEnvironment(environment => environment
                .UseModel(GraphModel
                    .FromBaseTypes<Vertex, Edge>()
                    .ConfigureVertices(model => model
                        .ConfigureMetadata(
                            typeof(RichVertex).GetProperty(nameof(RichVertex.Age))!,
                            metadata => new MemberMetadata(T.Key, metadata.SerializationBehaviour))))
                .ConfigureServerCompatibilityRange(_ => VersionRange.From(TinkerPopVersions.V3_8_2, TinkerPopVersions.V3_8_2)))
            .V<RichVertex>()
            .Where(x => x.Age, _ => _.Fold())
            .AsAdmin()
            .Steps[^1]
            .Should()
            .BeOfType<WhereTraversalStep>()
            .Which
            .Traversal[0]
            .Should()
            .BeOfType<ValuesStep>()
            .Which
            .Keys
            .Should()
            .Equal("key");

        [Fact]
        public void Lower_chain_from_value() => _g
            .Inject(0)
            .Lower();

        [Fact]
        public void Lower_chain_from_untyped_vertex() => _g
            .V()
            .Lower()
            .Lower()
            .Lower();

        [Fact]
        public void Lower_chain_from_typed_vertex() => _g
            .V<RichVertex>()
            .Lower()
            .Lower()
            .Lower();

        [Fact]
        public void Lower_chain_from_untyped_edge() => _g
            .E()
            .Lower()
            .Lower()
            .Lower();

        [Fact]
        public void Lower_chain_from_typed_edge() => _g
            .E<RichEdge>()
            .Lower()
            .Lower()
            .Lower();

        [Fact]
        public void Lower_chain_from_array() => _g
            .Inject(0)
            .Fold()
            .Lower();

        [Fact]
        public void Properties_on_id_throws() => _g
            .V<Vertex>()
            .Invoking(_ => _
                .Properties(x => x.Id!))
            .Should()
            .Throw<ExpressionNotSupportedException>();

        [Fact]
        public void No_parameterless_non_generic_method_throws()
        {
            var query = _g
                .Inject(0);

            query
                .GetType()
                .GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic)
                .Where(method => method.GetParameters().Length == 0)
                .Where(method => !method.IsGenericMethod)
                .Select(method => method
                    .Invoke(query, null))
                .Invoking(_ => _
                    .ToArray())
                .Should()
                .NotThrow();
        }

        [Fact]
        public void Order_by_traversal_through_the_builder_without_element_type()
        {
            IGremlinQueryBaseRec<IVertexGremlinQuery<RichVertex>> query = _g.V<RichVertex>();

            query
                .Order(b => b.By(__ => __.Out()))
                .Debug()
                .Should()
                .StartWith("g.V().hasLabel('RichVertex').order().by(__.out(),asc).");
        }

        [Fact]
        public void Order_by_traversal_descending_through_the_builder_without_element_type()
        {
            IGremlinQueryBaseRec<IVertexGremlinQuery<RichVertex>> query = _g.V<RichVertex>();

            query
                .Order(b => b.ByDescending(__ => __.Out()))
                .Debug()
                .Should()
                .StartWith("g.V().hasLabel('RichVertex').order().by(__.out(),desc).");
        }

        [Fact]
        public void Concat_traversal_array_on_a_string_query() => _g
            .Inject("a")
            .Concat(new Func<IStringGremlinQuery<string>, IGremlinQueryBase<string>>[] { __ => __.Constant("b") })
            .Debug()
            .Should()
            .Be("g.inject('a').concat(__.constant('b'))");

        [Fact]
        public void Project_dynamic_by_name_and_traversal() => _g
            .V<RichVertex>()
            .Project(p => p
                .ToDynamic()
                .By("out", __ => __.Out()))
            .Debug()
            .Should()
            .StartWith("g.V().hasLabel('RichVertex').project('out').by(__.out()).");

        [Fact]
        public void Project_dynamic_with_empty_projection_protection() => _g
            .V<RichVertex>()
            .Project(p => p
                .WithEmptyProjectionProtection()
                .ToDynamic()
                .By("out", __ => __.Out()))
            .Debug()
            .Should()
            .StartWith("g.V().hasLabel('RichVertex').project('out').by(__.out().limit(1).fold()).map(__.unfold().group().by(__.select(keys)).by(__.select(values).unfold())).");

        [Fact]
        public void Project_dynamic_by_name_and_member_expression() => _g
            .V<RichVertex>()
            .Project(p => p
                .ToDynamic()
                .By("age", x => x.Age))
            .Debug()
            .Should()
            .StartWith("g.V().hasLabel('RichVertex').project('age').by('Age')");
    }
}
