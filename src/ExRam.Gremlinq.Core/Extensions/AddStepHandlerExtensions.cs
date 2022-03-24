using System.Collections.Generic;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal static class AddStepHandlerExtensions
    {
        public static StepStack AddSteps<TStep>(this IAddStepHandler addStepHandler, StepStack stepStack, IEnumerable<TStep> steps, IGremlinQueryEnvironment env)
            where TStep : Step
        {
            foreach (var step in steps)
            {
                stepStack = addStepHandler.AddStep(stepStack, step, env);
            }

            return stepStack;
        }
    }
}
