using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Core.Transformation;
using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class QuerySerializationTest<TSerialized> : QueryExecutionTest
    {
        protected QuerySerializationTest(GremlinqTestFixture fixture, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(
            fixture,
            testOutputHelper,
            callerFilePath)
        {
        }

        public override Task Verify<TElement>(IGremlinQueryBase<TElement> query)
        {
            var env = query.AsAdmin().Environment;

            return Verify(env
                .Serializer
                .TransformTo<TSerialized>()
                .From(query, env));
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
                        .Add(Create<EStep, Step[]>((step, env, recurse) => recurse
                            .TransformTo<Step[]>()
                            .From(
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
                        .Add(Create<EStep, Step[]>((step, env, recurse) =>
                            new Step[]
                            {
                                new VStep(ImmutableArray<object>.Empty),
                                new OutEStep(ImmutableArray<string>.Empty)
                            }))))
                .E()
                .Verify();
        }

        protected override IImmutableList<Func<string, string>> Scrubbers() => base
            .Scrubbers()
            .ScrubGuids();
    }
}
