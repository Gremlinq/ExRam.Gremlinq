using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class EmptyGremlinqTestFixture : GremlinqTestFixture
    {
        public EmptyGremlinqTestFixture() : base(g.ConfigureEnvironment(_ => _))
        {
        }
    }
}
