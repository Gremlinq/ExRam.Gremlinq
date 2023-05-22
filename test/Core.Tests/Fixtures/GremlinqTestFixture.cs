using System.Collections.Immutable;

using ExRam.Gremlinq.Core.Execution;
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
            GremlinQuerySource = source
                .ConfigureEnvironment(env => env
                    .ConfigureExecutor(exe => exe
                        .CatchExecutionExceptions()));
        }


        protected virtual IImmutableList<Func<string, string>> Scrubbers() => ImmutableList<Func<string, string>>.Empty;

        public virtual async Task Verify<TElement>(IGremlinQueryBase<TElement> query) => await GremlinqTestBase.Current.Verify(query.Debug());

        public IGremlinQuerySource GremlinQuerySource { get; }
    }
}
