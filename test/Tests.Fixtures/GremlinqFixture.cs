using ExRam.Gremlinq.Core;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public abstract class GremlinqFixture : IDisposable, IAsyncLifetime
    {
        private sealed class EmptyGremlinqTestFixture : GremlinqFixture
        {
            public EmptyGremlinqTestFixture() : base(g.ConfigureEnvironment(_ => _))
            {
            }
        }

        public static readonly GremlinqFixture Empty = new EmptyGremlinqTestFixture();

        protected GremlinqFixture(IGremlinQuerySource source)
        {
            GremlinQuerySource = source;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public virtual async Task InitializeAsync()
        {
        }

        public virtual async Task DisposeAsync()
        {
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public IGremlinQuerySource GremlinQuerySource { get; }
    }
}
