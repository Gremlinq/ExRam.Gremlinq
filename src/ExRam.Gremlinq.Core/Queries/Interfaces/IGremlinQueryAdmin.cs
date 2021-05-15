using System;
using ExRam.Gremlinq.Core.Projections;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryAdmin
    {
        TTargetQuery ConfigureSteps<TTargetQuery>(Func<StepStack, StepStack> configurator) where TTargetQuery : IGremlinQueryBase;
        TTargetQuery AddStep<TTargetQuery>(Step step/*, Func<Projection, Projection> projectionTransformation*/) where TTargetQuery : IGremlinQueryBase;

        TTargetQuery ChangeQueryType<TTargetQuery>() where TTargetQuery : IGremlinQueryBase;

        IGremlinQuerySource GetSource();

        StepStack Steps { get; }
        QueryFlags Flags { get; }
        Type ElementType { get; }
        Projection Projection { get; }
        IGremlinQueryEnvironment Environment { get; }
    }
}
