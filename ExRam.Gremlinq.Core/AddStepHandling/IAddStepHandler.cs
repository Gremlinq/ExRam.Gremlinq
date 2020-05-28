using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public interface IAddStepHandler
    {
        IImmutableStack<Step> AddStep<TStep>(IImmutableStack<Step> steps, TStep step, IGremlinQueryEnvironment environment) where TStep : Step;

        IAddStepHandler Override<TStep>(Func<IImmutableStack<Step>, TStep, IGremlinQueryEnvironment, Func<IImmutableStack<Step>, TStep, IGremlinQueryEnvironment, IAddStepHandler, IImmutableStack<Step>>, IAddStepHandler, IImmutableStack<Step>> addStepHandler) where TStep : Step;
    }
}
