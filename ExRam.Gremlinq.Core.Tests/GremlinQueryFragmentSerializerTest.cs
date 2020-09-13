using System.Collections.Immutable;
using System.Threading.Tasks;
using Moq;
using System.Linq;
using VerifyXunit;
using Xunit;

namespace ExRam.Gremlinq.Core.Tests
{
    [UsesVerify]
    public class GremlinQueryFragmentSerializerTest
    {
        [Fact]
        public async Task Empty()
        {
            await Verifier.Verify(GremlinQueryFragmentSerializer.Identity
                .Serialize(new HasLabelStep(ImmutableArray.Create("label")), Mock.Of<IGremlinQueryEnvironment>()));
        }

        [Fact]
        public async Task Base_type()
        {
            await Verifier.Verify(GremlinQueryFragmentSerializer.Identity
                .Override<Step>((step, env, overridden, recurse) => new VStep(ImmutableArray.Create<object>("id")))
                .Serialize(new HasLabelStep(ImmutableArray.Create("label")), Mock.Of<IGremlinQueryEnvironment>()));
        }

        [Fact]
        public async Task Irrelevant()
        {
            await Verifier.Verify(GremlinQueryFragmentSerializer.Identity
                .Override<HasKeyStep>((step, env, overridden, recurse) => new HasLabelStep(ImmutableArray.Create("should not be here")))
                .Serialize(new HasLabelStep(ImmutableArray.Create("label")), Mock.Of<IGremlinQueryEnvironment>()));
        }

        [Fact]
        public async Task Override1()
        {
            await Verifier.Verify(GremlinQueryFragmentSerializer.Identity
                .Override<HasLabelStep>((step, env, overridden, recurse) => overridden(new HasLabelStep(step.Labels.Add("added label")), env, recurse))
                .Serialize(new HasLabelStep(ImmutableArray.Create("label")), Mock.Of<IGremlinQueryEnvironment>()));
        }

        [Fact]
        public async Task Override2()
        {
            await Verifier.Verify(GremlinQueryFragmentSerializer.Identity
                .Override<HasLabelStep>((step, env, overridden, recurse) => overridden(new HasLabelStep(step.Labels.Add("added label override 1")), env, recurse))
                .Override<HasLabelStep>((step, env, overridden, recurse) => overridden(new HasLabelStep(step.Labels.Add("added label override 2")), env, recurse))
                .Serialize(new HasLabelStep(ImmutableArray.Create("label")), Mock.Of<IGremlinQueryEnvironment>()));
        }

        [Fact]
        public async Task Recurse()
        {
            await Verifier.Verify(GremlinQueryFragmentSerializer.Identity
                .Override<HasLabelStep>((step, env, overridden, recurse) => recurse.Serialize(new VStep(ImmutableArray.Create<object>("id")), env))
                .Serialize(new HasLabelStep(ImmutableArray.Create("label")), Mock.Of<IGremlinQueryEnvironment>()));
        }

        [Fact]
        public async Task Recurse_to_previous_override()
        {
            await Verifier.Verify(GremlinQueryFragmentSerializer.Identity
                .Override<VStep>((step, env, overridden, recurse) => overridden(new VStep(step.Ids.Add("another id")), env, recurse))
                .Override<HasLabelStep>((step, env, overridden, recurse) => recurse.Serialize(new VStep(ImmutableArray.Create<object>("id")), env))
                .Serialize(new HasLabelStep(ImmutableArray.Create("label")), Mock.Of<IGremlinQueryEnvironment>()));
        }

        [Fact]
        public async Task Recurse_to_later_override()
        {
            await Verifier.Verify(GremlinQueryFragmentSerializer.Identity
                .Override<HasLabelStep>((step, env, overridden, recurse) => recurse.Serialize(new VStep(ImmutableArray.Create<object>("id")), env))
                .Override<VStep>((step, env, overridden, recurse) => overridden(new VStep(step.Ids.Add("another id")), env, recurse))
                .Serialize(new HasLabelStep(ImmutableArray.Create("label")), Mock.Of<IGremlinQueryEnvironment>()));
        }

        [Fact]
        public async Task AllSteps()
        {
            await Verifier.Verify(
                TypeSystemTest.AllSteps
                    .Select(step => (
                        step.GetType(),
                        GremlinQueryFragmentSerializer.Default
                            .Serialize(step, GremlinQueryEnvironment.Empty)))
                    .ToArray());
        }
    }
}
