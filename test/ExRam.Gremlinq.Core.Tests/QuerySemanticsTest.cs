using System;
using System.Threading.Tasks;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class QuerySemanticsTest : GremlinqTestBase
    {
        private readonly IGremlinQuerySource _g;

        public QuerySemanticsTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _g = g
                .ConfigureEnvironment(x => x.UseModel(GraphModel
                    .FromBaseTypes<Vertex, Edge>()));
        }

        [Fact]
        public virtual Task Coalesce_with_2_subQueries_has_right_semantics()
        {
            return Verify(_g
                .V()
                .Coalesce(
                    _ => _.Out(),
                    _ => _.In())
                .AsAdmin()
                .Semantics);
        }

        [Fact]
        public virtual Task Coalesce_with_2_not_matching_subQueries_has_right_semantics()
        {
            return Verify(_g
                .V()
                .Coalesce(
                    _ => _.OutE(),
                    _ => _.In())
                .AsAdmin()
                .Semantics);
        }

        [Fact]
        public void QuerySemantics_should_not_be_marked_Flags_by_any_review()
        {
            typeof(QuerySemantics)
                .GetCustomAttributes(typeof(FlagsAttribute), true)
                .Should()
                .BeEmpty();
        }

        [Fact]
        public virtual Task ForceEdge_will_not_preserve_Vertex()
        {
            return Verify(_g
                .V()
                .ForceEdge()
                .AsAdmin()
                .Semantics);
        }

        [Fact]
        public virtual Task ForceElement_will_preserve_Vertex()
        {
            return Verify(_g
                .V()
                .ForceElement()
                .AsAdmin()
                .Semantics);
        }

        [Fact]
        public virtual Task ForceValue_will_not_preserve_Vertex()
        {
            return Verify(_g
                .V()
                .ForceValue()
                .AsAdmin()
                .Semantics);
        }

        [Fact]
        public virtual Task Unfold_will_get_Vertex_back()
        {
            return Verify(_g
                .V()
                .Fold()
                .Unfold()
                .AsAdmin()
                .Semantics);
        }
    }
}
