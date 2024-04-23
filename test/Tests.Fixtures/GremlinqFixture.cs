using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public class GremlinqFixture : IAsyncLifetime
    {
        private IGremlinQuerySource? _g;

        protected virtual async Task<IGremlinQuerySource> TransformQuerySource(IGremlinQuerySource g) => g;

        public virtual async Task InitializeAsync()
        {
            _g = await TransformQuerySource(g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(options => options
                        .SetValue(GremlinqOption.StringComparisonTranslationStrictness,
                            StringComparisonTranslationStrictness.Lenient))));

            _g = _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model == GraphModel.Invalid
                        ? GraphModel.FromBaseTypes<Vertex, Edge>()
                        : model));
        }

        public virtual async Task DisposeAsync()
        {
            if (_g is IAsyncDisposable disposable)
                await disposable.DisposeAsync();
        }

        public IGremlinQuerySource Source => _g ?? throw new InvalidOperationException();
    }
}
