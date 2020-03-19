using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuerySource : IConfigurableGremlinQuerySource, IStartGremlinQuery
    {
        IGremlinQuerySource RemoveStrategies(params Type[] strategyTypes);

        IGremlinQueryEnvironment Environment { get; }
        ImmutableList<Type> ExcludedStrategyTypes { get; }
    }

    public interface IConfigurableGremlinQuerySource
    {
        IGremlinQuerySource ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation);
    }
}
