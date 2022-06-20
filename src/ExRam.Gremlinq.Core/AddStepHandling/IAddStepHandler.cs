using System;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    public interface IAddStepHandler
    {
        Traversal AddStep<TStep>(Traversal steps, TStep step, IGremlinQueryEnvironment environment) where TStep : Step;

        IAddStepHandler Override<TStep>(Func<Traversal, TStep, IGremlinQueryEnvironment, Func<Traversal, TStep, IGremlinQueryEnvironment, IAddStepHandler, Traversal>, IAddStepHandler, Traversal> addStepHandler) where TStep : Step;
    }
}
