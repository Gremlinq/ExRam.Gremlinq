using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Tests
{
    public class BytecodeQuerySerializationTest : QueryExecutionTest, IClassFixture<GremlinqFixture>
    {
        public BytecodeQuerySerializationTest(GremlinqFixture fixture) : base(fixture, new SerializingVerifier<Bytecode>())
        {
        }

        [Fact]
        public virtual Task Drop() => _g
            .V<Person>()
            .Drop()
            .Verify();

        [Fact]
        public virtual Task Drop_in_local() => _g
            .Inject(1)
            .Local(__ => __
                .V()
                .Drop())
            .Verify();

        [Fact]
        public Task Traversal_detour_serialization() => _g
            .ConfigureEnvironment(env => env
                .ConfigureSerializer(ser => ser
                    .Add(ConverterFactory.Create<EStep, Traversal>((_, _, _, _) => Traversal.Create(
                        2,
                        0,
                        (span, _) => 
                        {
                            span[0] = new VStep(ImmutableArray<object>.Empty);
                            span[1] = new OutEStep(ImmutableArray<string>.Empty);
                        })))))
            .E()
            .Verify();

    }
}
