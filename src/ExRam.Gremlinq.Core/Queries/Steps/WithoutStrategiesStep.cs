using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class WithoutStrategiesStep : Step
    {
        public WithoutStrategiesStep(ImmutableArray<Type> strategyTypes) : base()
        {
            StrategyTypes = strategyTypes;
        }

        public ImmutableArray<Type> StrategyTypes { get; }
    }
}
