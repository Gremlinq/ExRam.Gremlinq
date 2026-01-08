using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class RealNeptuneAssertions : GremlinqTestBase, IClassFixture<NeptuneFixture>
    {
        private readonly IGremlinQuerySource _source;

        public RealNeptuneAssertions(NeptuneFixture fixture) : base(new ExecutingVerifier())
        {
            _source = fixture
                .GetQuerySource();
        }

        [Fact(Explicit = true)]
        public Task Native_TimeSpan() => _source
            .Inject(TimeSpan.FromMinutes(10))
            .Verify();
    }
}
