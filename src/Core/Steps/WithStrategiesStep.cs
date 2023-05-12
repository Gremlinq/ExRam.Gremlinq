using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class WithStrategiesStep : Step
    {
        public WithStrategiesStep(ImmutableArray<IGremlinQueryStrategy> strategies)
        {
            Strategies = strategies;
        }

        public ImmutableArray<IGremlinQueryStrategy> Strategies { get; }
    }
}
