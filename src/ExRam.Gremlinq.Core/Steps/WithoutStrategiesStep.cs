using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class WithoutStrategiesStep : Step
    {
        public WithoutStrategiesStep(ImmutableArray<Type> strategyTypes)
        {
            StrategyTypes = strategyTypes;
        }

        public ImmutableArray<Type> StrategyTypes { get; }
    }
}
