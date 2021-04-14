using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ExRam.Gremlinq.Tests.Entities;
using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class QuerySerializationTest : QueryExecutionTest
    {
        protected QuerySerializationTest(IGremlinqTestFixture fixture, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(
            fixture
                .Configure(g => g
                    .ConfigureEnvironment(env => env
                        .UseExecutor(GremlinQueryExecutor.Identity))),
            testOutputHelper,
            callerFilePath)
        {
        }

        [Fact]
        public async Task StringKey()
        {
            await _g
                .V<Person>("id")
                .Verify();
        }

        [Fact]
        public async Task Multi_step_serialization()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureSerializer(ser => ser
                        .ConfigureFragmentSerializer(f => f
                            .Override<EStep>((step, env, overridden, recurse) => recurse.Serialize(
                                new Step[]
                                {
                                    new VStep(ImmutableArray<object>.Empty),
                                    new OutEStep(ImmutableArray<string>.Empty)
                                },
                                env)))))
                .E()
                .Verify();
        }

        [Fact]
        public async Task Multi_step_serialization_with_forgotten_serialize()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureSerializer(ser => ser
                        .ConfigureFragmentSerializer(f => f
                            .Override<EStep>((step, env, overridden, recurse) =>
                                new Step[]
                                {
                                    new VStep(ImmutableArray<object>.Empty),
                                    new OutEStep(ImmutableArray<string>.Empty)
                                }))))
                .E()
                .Verify();
        }
    }
}
