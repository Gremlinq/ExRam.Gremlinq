using System;
using System.Linq;
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
        public Task Mappings()
        {
            return Verify(typeof(IGremlinQueryBase)
                .Assembly
                .GetTypes()
                .Where(x => x.IsInterface)
                .ToDictionary(x => x, x => x.TryGetQuerySemanticsFromQueryType()));
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
        public void LowestCommon()
        {
            var values = (QuerySemantics[])Enum.GetValues(typeof(QuerySemantics));

            foreach (var value1 in values)
            {
                foreach (var value2 in values)
                {
                    Enum
                        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
                        .IsDefined(value1 & value2)
                        .Should()
                        .BeTrue();
                }
            }
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
        public virtual Task ForceValue_preserves_Vertex_when_determined_from_element_type()
        {
            return Verify(_g
                .V<Person>()
                .ForceValue()
                .AsAdmin()
                .Semantics);
        }

        [Fact]
        public virtual Task ForceValue_cannot_preserve_Vertex_when_just_object()
        {
            return Verify(_g
                .V()
                .ForceValue()
                .AsAdmin()
                .Semantics);
        }
    }
}
