using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public abstract class GremlinqFixture : IAsyncLifetime
    {
        private sealed class EmptyGremlinqTestFixture : GremlinqFixture
        {
            protected override async Task<IGremlinQuerySource> TransformQuerySource(IGremlinQuerySource g) => g;
        }

        public static readonly GremlinqFixture Empty = new EmptyGremlinqTestFixture();

        private static readonly TaskCompletionSource<IGremlinQuerySource> Disposed = new ();
        private TaskCompletionSource<IGremlinQuerySource>? _lazyQuerySource;

        static GremlinqFixture()
        {
            Disposed.TrySetException(new ObjectDisposedException(nameof(GremlinqFixture)));
        }

        protected abstract Task<IGremlinQuerySource> TransformQuerySource(IGremlinQuerySource g);

        public Task<IGremlinQuerySource> GremlinQuerySource => GetGremlinQuerySource();

        private async Task<IGremlinQuerySource> GetGremlinQuerySource()
        {
            if (Volatile.Read(ref _lazyQuerySource) is { } tcs)
                return await tcs.Task;

            var newTcs = new TaskCompletionSource<IGremlinQuerySource>();

            if (Interlocked.CompareExchange(ref _lazyQuerySource, newTcs, null) == null)
            {
                try
                {
                    var g1 = await TransformQuerySource(g
                        .ConfigureEnvironment(env => env
                            .ConfigureOptions(options => options
                                .SetValue(GremlinqOption.StringComparisonTranslationStrictness, StringComparisonTranslationStrictness.Lenient))));

                    newTcs.TrySetResult(g1
                        .ConfigureEnvironment(env => env
                            .ConfigureModel(model => model == GraphModel.Invalid
                                ? GraphModel.FromBaseTypes<Vertex, Edge>()
                                : model)));
                }
                catch (Exception ex)
                {
                    newTcs.TrySetException(ex);

                    Interlocked.CompareExchange(ref _lazyQuerySource, null, newTcs);
                }

                return await newTcs.Task;
            }

            return await GetGremlinQuerySource();
        }

        public virtual async Task InitializeAsync()
        {
           
        }

        public virtual async Task DisposeAsync()
        {
            if (Interlocked.Exchange(ref _lazyQuerySource, Disposed) is { } tcs && tcs != Disposed)
            {
                if (await tcs.Task is IAsyncDisposable disposable)
                    await disposable.DisposeAsync();
            }
        }
    }
}
