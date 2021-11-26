using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Tests.Entities;
using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class QuerySerializationTest : QueryExecutionTest
    {
        public abstract class Fixture : GremlinqTestFixture
        {
            protected Fixture(IGremlinQuerySource source) : base(source
                .ConfigureEnvironment(env => env
                    .UseExecutor(GremlinQueryExecutor.Identity)))
            {
            }
        }

        private static readonly Regex GuidRegex = new("[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}", RegexOptions.IgnoreCase);

        protected QuerySerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(
            fixture,
            testOutputHelper,
            callerFilePath)
        {
        }

        public override IImmutableList<Func<string, string>> Scrubbers()
        {
            return base.Scrubbers()
                .Add(x => GuidRegex.Replace(x, "Scrubbed GUID"));
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
