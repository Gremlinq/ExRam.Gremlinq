using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuerySource : IStartGremlinQuery
    {
        IGremlinQuerySource ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation);
        IGremlinQuerySource AddStrategies(params IGremlinQueryStrategy[] strategies);
        IGremlinQuerySource RemoveStrategies(params Type[] strategyTypes);

        IGremlinQueryEnvironment Environment { get; }
        ImmutableList<Type> ExcludedStrategyTypes { get; }
        ImmutableList<IGremlinQueryStrategy> IncludedStrategies { get; }
    }
}
