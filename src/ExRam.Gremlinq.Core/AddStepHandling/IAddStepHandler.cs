using System;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    public interface IAddStepHandler
    {
        StepStack AddStep<TStep>(StepStack steps, TStep step, IGremlinQueryEnvironment environment) where TStep : Step;

        IAddStepHandler Override<TStep>(Func<StepStack, TStep, IGremlinQueryEnvironment, Func<StepStack, TStep, IGremlinQueryEnvironment, IAddStepHandler, StepStack>, IAddStepHandler, StepStack> addStepHandler) where TStep : Step;
    }
}
