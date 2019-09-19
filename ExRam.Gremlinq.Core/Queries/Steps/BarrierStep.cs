using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class BarrierStep : Step
    {
        public static readonly BarrierStep Instance = new BarrierStep();
    }
}
