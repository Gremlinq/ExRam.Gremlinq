using System.Collections.Generic;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal static class AddStepHandlerExtensions
    {
        public static Traversal AddSteps<TStep>(this IAddStepHandler addStepHandler, Traversal traversal, IEnumerable<TStep> steps, IGremlinQueryEnvironment env)
            where TStep : Step
        {
            foreach (var step in steps)
            {
                traversal = addStepHandler.AddStep(traversal, step, env);
            }

            return traversal;
        }
    }
}
