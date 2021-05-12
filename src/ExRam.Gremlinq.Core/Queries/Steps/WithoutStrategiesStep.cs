using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class WithoutStrategiesStep : Step
    {
        public WithoutStrategiesStep(ImmutableArray<Type> strategyTypes, QuerySemantics? semantics = default) : base(semantics)
        {
            StrategyTypes = strategyTypes;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new WithoutStrategiesStep(StrategyTypes, semantics);

        public ImmutableArray<Type> StrategyTypes { get; }
    }
}
