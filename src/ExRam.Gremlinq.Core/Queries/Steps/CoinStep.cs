namespace ExRam.Gremlinq.Core
{
    public sealed class CoinStep : Step, IIsOptimizableInWhere
    {
        public CoinStep(double probability)
        {
            Probability = probability;
        }

        public double Probability { get; }
    }
}
