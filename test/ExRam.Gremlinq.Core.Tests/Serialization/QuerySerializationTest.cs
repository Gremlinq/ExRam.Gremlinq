using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Tests.Entities;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class QuerySerializationTest : QueryExecutionTest
    {
        protected QuerySerializationTest(GremlinqTestFixture fixture, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(
            fixture,
            testOutputHelper,
            callerFilePath)
        {
        }

        public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => base.Verify(query.Cast<object>());

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
                        .Add<EStep>((step, env, recurse) => recurse.Serialize(
                            new Step[]
                            {
                                new VStep(ImmutableArray<object>.Empty),
                                new OutEStep(ImmutableArray<string>.Empty)
                            },
                            env))))
                .E()
                .Verify();
        }

        [Fact]
        public async Task Multi_step_serialization_with_forgotten_serialize()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureSerializer(ser => ser
                        .Add<EStep>((step, env, recurse) =>
                            new Step[]
                            {
                                new VStep(ImmutableArray<object>.Empty),
                                new OutEStep(ImmutableArray<string>.Empty)
                            })))
                .E()
                .Verify();
        }

        protected override IImmutableList<Func<string, string>> Scrubbers() => base
            .Scrubbers()
            .ScrubGuids();
    }
}
