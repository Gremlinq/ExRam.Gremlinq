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
            .V<RichVertex>()
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
        public Task Generated_step_label_after_named_step_label() => _g
            .WithSideEffect("sideEffectLabel", 36)
            .WithSideEffect(new StepLabel<int>(), 37)
            .V()
            .Verify();

        [Fact]
        public Task Named_step_label_between_generated_step_labels() => _g
            .WithSideEffect(new StepLabel<int>(), 1)
            .WithSideEffect("sideEffectLabel", 2)
            .WithSideEffect(new StepLabel<int>(), 3)
            .V()
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
