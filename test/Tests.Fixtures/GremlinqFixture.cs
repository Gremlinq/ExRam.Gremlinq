using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class EmptyGremlinqTestFixture : GremlinqFixture
    {
        protected override async Task<IGremlinQuerySource> TransformQuerySource(IGremlinQuerySource g) => g;
    }

    public abstract class GremlinqFixture : IAsyncLifetime
    {
        private IGremlinQuerySource? _gremlinQuerySource;
        
        protected abstract Task<IGremlinQuerySource> TransformQuerySource(IGremlinQuerySource g);

        public IGremlinQuerySource GremlinQuerySource => _gremlinQuerySource ?? throw new InvalidOperationException();// GetGremlinQuerySource();

        public virtual async Task InitializeAsync()
        {
            _gremlinQuerySource = await TransformQuerySource(g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(options => options
                        .SetValue(GremlinqOption.StringComparisonTranslationStrictness,
                            StringComparisonTranslationStrictness.Lenient))));

            _gremlinQuerySource = _gremlinQuerySource
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model == GraphModel.Invalid
                        ? GraphModel.FromBaseTypes<Vertex, Edge>()
                        : model));
        }

        public virtual async Task DisposeAsync()
        {
            if (_gremlinQuerySource is IAsyncDisposable disposable)
                await disposable.DisposeAsync();
        }
    }
}
