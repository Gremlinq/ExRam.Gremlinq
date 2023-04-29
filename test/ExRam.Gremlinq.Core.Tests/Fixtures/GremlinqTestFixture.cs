using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GremlinqTestFixture
    {
        private sealed class EmptyGremlinqTestFixture : GremlinqTestFixture
        {
            public EmptyGremlinqTestFixture() : base(g.ConfigureEnvironment(_ => _))
            {
            }
        }

        public static readonly GremlinqTestFixture Empty = new EmptyGremlinqTestFixture();

        protected GremlinqTestFixture(IGremlinQuerySource source)
        {
            GremlinQuerySource = source;
        }

        public IGremlinQuerySource GremlinQuerySource { get; }
    }
}
