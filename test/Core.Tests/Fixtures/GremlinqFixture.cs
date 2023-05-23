using ExRam.Gremlinq.Core.Execution;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GremlinqFixture
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
            GremlinQuerySource = source
                .ConfigureEnvironment(env => env
                    .ConfigureExecutor(exe => exe
                        .CatchExecutionExceptions()));
        }

        public IGremlinQuerySource GremlinQuerySource { get; }
    }
}
