using System.Collections.Immutable;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace ExRam.Gremlinq.Core.Tests
{
    [UsesVerify]
    public class AddStepHandlerTest
    {
        [Fact]
        public async Task Empty()
        {
            await Verifier.Verify(AddStepHandler.Empty
                .AddStep(ImmutableStack<Step>.Empty, new HasLabelStep(ImmutableArray.Create("label")), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Base_type()
        {
            await Verifier.Verify(AddStepHandler.Empty
                .Override<Step>((steps, step, env, overridden, recurse) => steps.Push(new VStep(ImmutableArray.Create<object>("id"))))
                .AddStep(ImmutableStack<Step>.Empty, new HasLabelStep(ImmutableArray.Create("label")), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Irrelevant()
        {
            await Verifier.Verify(AddStepHandler.Empty
                .Override<HasKeyStep>((steps, step, env, overridden, recurse) => steps.Push(new HasLabelStep(ImmutableArray.Create("should not be here"))))
                .AddStep(ImmutableStack<Step>.Empty, new HasLabelStep(ImmutableArray.Create("label")), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Override1()
        {
            await Verifier.Verify(AddStepHandler.Empty
                .Override<HasLabelStep>((steps, step, env, overridden, recurse) => overridden(steps, new HasLabelStep(step.Labels.Add("added label")), env, recurse))
                .AddStep(ImmutableStack<Step>.Empty, new HasLabelStep(ImmutableArray.Create("label")), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Override2()
        {
            await Verifier.Verify(AddStepHandler.Empty
                .Override<HasLabelStep>((steps, step, env, overridden, recurse) => overridden(steps, new HasLabelStep(step.Labels.Add("added label override 1")), env, recurse))
                .Override<HasLabelStep>((steps, step, env, overridden, recurse) => overridden(steps, new HasLabelStep(step.Labels.Add("added label override 2")), env, recurse))
                .AddStep(ImmutableStack<Step>.Empty, new HasLabelStep(ImmutableArray.Create("label")), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Recurse()
        {
            await Verifier.Verify(AddStepHandler.Empty
                .Override<HasLabelStep>((steps, step, env, overridden, recurse) => recurse.AddStep(steps, new VStep(ImmutableArray.Create<object>("id")), env))
                .AddStep(ImmutableStack<Step>.Empty, new HasLabelStep(ImmutableArray.Create("label")), GremlinQueryEnvironment.Empty));
        }


        [Fact]
        public async Task Recurse_to_previous_override()
        {
            await Verifier.Verify(AddStepHandler.Empty
                .Override<VStep>((steps, step, env, overridden, recurse) => overridden(steps, new VStep(step.Ids.Add("another id")), env, recurse))
                .Override<HasLabelStep>((steps, step, env, overridden, recurse) => recurse.AddStep(steps, new VStep(ImmutableArray.Create<object>("id")), env))
                .AddStep(ImmutableStack<Step>.Empty, new HasLabelStep(ImmutableArray.Create("label")), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Recurse_to_later_override()
        {
            await Verifier.Verify(AddStepHandler.Empty
                .Override<HasLabelStep>((steps, step, env, overridden, recurse) => recurse.AddStep(steps, new VStep(ImmutableArray.Create<object>("id")), env))
                .Override<VStep>((steps, step, env, overridden, recurse) => overridden(steps, new VStep(step.Ids.Add("another id")), env, recurse))
                .AddStep(ImmutableStack<Step>.Empty, new HasLabelStep(ImmutableArray.Create("label")), GremlinQueryEnvironment.Empty));
        }
    }
}
