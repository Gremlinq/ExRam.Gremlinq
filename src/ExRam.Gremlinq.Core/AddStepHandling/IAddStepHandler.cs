using System;

namespace ExRam.Gremlinq.Core
{
    public interface IAddStepHandler
    {
        StepStack AddStep<TStep>(StepStack steps, TStep step, QuerySemantics? semantics, IGremlinQueryEnvironment environment) where TStep : Step;

        IAddStepHandler Override<TStep>(Func<StepStack, TStep, QuerySemantics?, IGremlinQueryEnvironment, Func<StepStack, TStep, QuerySemantics?, IGremlinQueryEnvironment, IAddStepHandler, StepStack>, IAddStepHandler, StepStack> addStepHandler) where TStep : Step;
    }
}
