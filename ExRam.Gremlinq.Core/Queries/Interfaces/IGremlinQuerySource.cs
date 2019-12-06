using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuerySource : IGremlinQueryBase
    {
        IGremlinQuerySource UseName(string name);
        IGremlinQuerySource ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation);
        IGremlinQuerySource AddStrategies(params IGremlinQueryStrategy[] strategies);
        IGremlinQuerySource RemoveStrategies(params Type[] strategyTypes);

        string Name { get; }
        IGremlinQueryEnvironment Environment { get; }
        ImmutableList<Type> ExcludedStrategyTypes { get; }
        ImmutableList<IGremlinQueryStrategy> IncludedStrategies { get; }
    }
}
