using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public interface IConfigurableGremlinQuerySource : IGremlinQuerySource
    {
        IConfigurableGremlinQuerySource UseName(string name);
        IConfigurableGremlinQuerySource ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation);
        IConfigurableGremlinQuerySource AddStrategies(params IGremlinQueryStrategy[] strategies);
        IConfigurableGremlinQuerySource RemoveStrategies(params Type[] strategyTypes);

        string Name { get; }
        IGremlinQueryEnvironment Environment { get; }
        ImmutableList<Type> ExcludedStrategyTypes { get; }
        ImmutableList<IGremlinQueryStrategy> IncludedStrategies { get; }
    }
}
