using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.WebSocket;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class GremlinServerDebugGremlinQuerySerializationTest : DebugGremlinQuerySerializationTest, IClassFixture<GremlinServerDebugGremlinQuerySerializationTest.Fixture>
    {
        public new sealed class Fixture : GremlinqTestFixture
        {
            public Fixture() : base(g
                .UseGremlinServer(_ => _
                    .AtLocalhost())
                .ConfigureEnvironment(env => env
                    .UseExecutor(GremlinQueryExecutor.Create((query, env) =>
                    {
                        return AsyncEnumerable.Create(Core);

                        async IAsyncEnumerator<object> Core(CancellationToken ct)
                        {
                            yield return env.Debugger.TryToString(query, env)!;
                        }
                    }))))
            {
            }
        }

        public GremlinServerDebugGremlinQuerySerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
