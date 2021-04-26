using System;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryAdmin
    {
        TTargetQuery ConfigureSteps<TTargetQuery>(Func<StepStack, StepStack> configurator) where TTargetQuery : IGremlinQueryBase;
        TTargetQuery AddStep<TTargetQuery>(Step step) where TTargetQuery : IGremlinQueryBase;

        TTargetQuery ChangeQueryType<TTargetQuery>() where TTargetQuery : IGremlinQueryBase;

        IGremlinQuerySource GetSource();

        Traversal ToTraversal();

        StepStack Steps { get; }
        Type ElementType { get; }
        QuerySemantics Semantics { get; }
        IGremlinQueryEnvironment Environment { get; }
    }
}
