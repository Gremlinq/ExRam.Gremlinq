using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryAdmin
    {
        IGremlinQuery<object> ConfigureSteps(Func<IImmutableStack<Step>, IImmutableStack<Step>> configurator);
        TTargetQuery ChangeQueryType<TTargetQuery>() where TTargetQuery : IGremlinQueryBase;

        IImmutableStack<Step> Steps { get; }
        IEnumerable<Step> EffectiveSteps { get; }
        IGremlinQueryEnvironment Environment { get; }
    }
}
