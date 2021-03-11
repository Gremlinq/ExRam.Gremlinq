using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class QuerySemanticsTest : GremlinqTestBase
    {
        public QuerySemanticsTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public Task Mappings()
        {
            return Verify(typeof(IGremlinQueryBase)
                .Assembly
                .GetTypes()
                .Where(x => x.IsInterface)
                .ToDictionary(x => x, x => x.TryGetQuerySemantics()));
        }

        [Fact]
        public virtual Task Coalesce_with_2_subQueries_has_right_semantics()
        {
            return Verify(g
                .ConfigureEnvironment(_ => _)
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
            return Verify(g
                .ConfigureEnvironment(_ => _)
                .V()
                .Coalesce(
                    _ => _.OutE(),
                    _ => _.In())
                .AsAdmin()
                .Semantics);
        }
    }
}
