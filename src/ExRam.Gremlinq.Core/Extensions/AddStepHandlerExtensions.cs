using System.Collections.Generic;

using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core.Queries
{
    //targetQuery
    //    .Continue()                     //ContinuationBuilder
    //    .FromSource(sourceQuery)        //ContinuationBuilder mit anderer Source
    //    .With(continuation1)            //SingleConrinuationBuilder
    //    .With(continuation2)            //MultiContinuationBuilder
    //    .WithSteps(targetQuery / targetQueries => Step / Step[]) // 2 Overloads für Single, 2 für Multi
    //    .As<TTargetQuery>()             //-> TargetQuery
    //    .AsSourceQuery

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
