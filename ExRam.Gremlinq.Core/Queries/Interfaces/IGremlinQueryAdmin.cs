using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryAdmin
    {
        TTargetQuery ConfigureSteps<TTargetQuery>(Func<IImmutableStack<Step>, IImmutableStack<Step>> configurator) where TTargetQuery : IGremlinQueryBase;
        TTargetQuery AddStep<TTargetQuery>(Step step) where TTargetQuery : IGremlinQueryBase;

        TTargetQuery ChangeQueryType<TTargetQuery>() where TTargetQuery : IGremlinQueryBase;

        Traversal ToTraversal();

        Type ElementType { get; }
        IImmutableStack<Step> Steps { get; }
        IGremlinQueryEnvironment Environment { get; }
    }
}
