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

        private readonly Lazy<Task<IGremlinQuerySource>> _lazyGremlinQuerySource;

        protected GremlinqFixture(IGremlinQuerySource source) : this(async () => source)
        {
        }

        protected GremlinqFixture(Func<Task<IGremlinQuerySource>> sourceFactory)
        {
            _lazyGremlinQuerySource = new Lazy<Task<IGremlinQuerySource>>(sourceFactory, LazyThreadSafetyMode.ExecutionAndPublication);
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

        public Task<IGremlinQuerySource> GremlinQuerySource { get => _lazyGremlinQuerySource.Value; }
    }
}
