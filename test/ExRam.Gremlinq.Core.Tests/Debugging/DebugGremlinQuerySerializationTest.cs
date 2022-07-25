﻿using ExRam.Gremlinq.Core.Execution;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class DebugGremlinQuerySerializationTest : QueryExecutionTest
    {
        public sealed class Fixture : GremlinqTestFixture
        {
            public Fixture() : base(g
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

        public DebugGremlinQuerySerializationTest(GremlinqTestFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }

        public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => base.Verify(query.Cast<string>());
    }
}
