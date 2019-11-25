using System;

namespace ExRam.Gremlinq.Core
{
    public sealed class WithoutStrategiesStep : Step
    {
        public WithoutStrategiesStep(Type[] strategyTypes)
        {
            StrategyTypes = strategyTypes;
        }

        public Type[] StrategyTypes { get; }
    }
}
