using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public interface IAddStepHandler
    {
        IImmutableStack<Step> AddStep<TStep>(IImmutableStack<Step> steps, TStep step) where TStep : Step;

        IAddStepHandler Override<TStep>(Func<IImmutableStack<Step>, TStep, Func<IImmutableStack<Step>, TStep, IImmutableStack<Step>>, IAddStepHandler, IImmutableStack<Step>> addStepHandler) where TStep : Step;
    }
}
