namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class CoinStep : Step, IIsOptimizableInWhere
    {
        public CoinStep(double probability) : base()
        {
            Probability = probability;
        }

        public double Probability { get; }
    }
}
