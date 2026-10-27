using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class ProjectionTest
    {
        private readonly IGremlinQuerySource _g;

        public ProjectionTest()
        {
            _g = g
                .ConfigureEnvironment(x => x.UseModel(GraphModel
                    .FromBaseTypes<Vertex, Edge>()));
        }

        [Fact]
        public virtual Task Coalesce_with_2_subQueries_has_right_semantics() => Verify(_g
            .V()
            .Coalesce(
                _ => _.Out(),
                _ => _.In())
            .ToTraversal()
            .Projection);

        [Fact]
        public virtual Task Coalesce_with_2_not_matching_subQueries_has_right_semantics() => Verify(_g
            .V()
            .Coalesce(
                _ => _.OutE(),
                _ => _.In())
            .ToTraversal()
            .Projection);

        [Fact]
        public virtual Task Coalesce_with_single_subQuery_with_limit_has_right_semantics() => Verify(_g
            .V()
            .Coalesce(
                _ => _
                    .OutE()
                    .Limit(1))
            .ToTraversal()
            .Projection);

        [Fact]
        public virtual Task ForceEdge_will_not_preserve_Vertex() => Verify(_g
           .V()
           .ForceEdge()
           .ToTraversal()
           .Projection);

        [Fact]
        public virtual Task ForceElement_will_preserve_Vertex() => Verify(_g
            .V()
            .ForceElement()
            .ToTraversal()
            .Projection);

        [Fact]
        public virtual Task ForceValue_will_not_preserve_Vertex() => Verify(_g
            .V()
            .ForceValue()
            .ToTraversal()
            .Projection);

        [Fact]
        public virtual Task Unfold_will_get_Vertex_back() => Verify(_g
            .V()
            .Fold()
            .Unfold()
            .ToTraversal()
            .Projection);

        [Fact]
        public virtual Task Where_variable_bool_property_true()
        {
            var anon = new
            {
                Prop = true
            };

            return Verify(_g
                .V()
                .Where(x => anon.Prop)
                .ToTraversal()
                .Projection);
        }

        [Fact]
        public virtual Task Where_variable_bool_property_false()
        {
            var anon = new
            {
                Prop = true
            };

            return Verify(_g
                .V()
                .Where(x => anon.Prop)
                .ToTraversal()
                .Projection);
        }

        [Fact]
        public virtual Task Coalesce_Where_variable_bool_property_false()
        {
            var anon = new
            {
                Prop = false
            };

            return Verify(_g
                .V()
                .Coalesce(
                    __ => __,
                    __ => __
                        .Where(x => anon.Prop))
                .ToTraversal()
                .Projection);
        }

        [Fact]
        public void Lowest_of_a_vertex_and_a_vertex_property_is_an_element() => Projection.VertexProperty
            .Lowest(Projection.Vertex)
            .Should()
            .BeSameAs(Projection.Element);

        [Fact]
        public void Lowest_of_an_array_and_a_value_is_empty() => Projection.Value
            .Fold()
            .Lowest(Projection.Value)
            .Should()
            .BeSameAs(Projection.Empty);

        [Fact]
        public void Lowest_of_a_group_and_a_value_is_empty() => Projection.Value
            .Group(Projection.Value, Projection.Value)
            .Lowest(Projection.Value)
            .Should()
            .BeSameAs(Projection.Empty);

        [Fact]
        public void Lowest_of_a_tuple_and_a_value_is_empty() => Projection.Value
            .Project(new ProjectStep(["a", "b"]), new ProjectStep.ByStep[] { new ProjectStep.ByKeyStep("a"), new ProjectStep.ByKeyStep("b") })
            .Lowest(Projection.Value)
            .Should()
            .BeSameAs(Projection.Empty);

        [Fact]
        public void Select_of_an_unknown_key_from_a_tuple_is_empty() => Projection.Value
            .Project(new ProjectStep(["a", "b"]), new ProjectStep.ByStep[] { new ProjectStep.ByKeyStep("a"), new ProjectStep.ByKeyStep("b") })
            .Select(["c"])
            .Should()
            .BeSameAs(Projection.Empty);

        [Fact]
        public void Select_of_two_keys_from_a_tuple_is_a_tuple() => Projection.Value
            .Project(new ProjectStep(["a", "b", "c"]), new ProjectStep.ByStep[] { new ProjectStep.ByKeyStep("a"), new ProjectStep.ByKeyStep("b"), new ProjectStep.ByKeyStep("c") })
            .Select(["a", "b"])
            .Should()
            .BeOfType<TupleProjection>();
    }
}
