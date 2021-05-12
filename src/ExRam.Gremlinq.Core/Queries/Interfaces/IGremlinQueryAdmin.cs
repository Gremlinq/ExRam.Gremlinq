using System;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryAdmin
    {
        TTargetQuery ConfigureSteps<TTargetQuery>(Func<StepStack, StepStack> configurator) where TTargetQuery : IGremlinQueryBase;
        TTargetQuery AddStep<TTargetQuery>(Step step) where TTargetQuery : IGremlinQueryBase;

        TTargetQuery ChangeQueryType<TTargetQuery>() where TTargetQuery : IGremlinQueryBase;

        IGremlinQuerySource GetSource();

        StepStack Steps { get; }
        QueryFlags Flags { get; }
        Type ElementType { get; }
        IGremlinQueryEnvironment Environment { get; }
    }
}
