namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class CoinStep : Step, IFilterStep
    {
        public CoinStep(double probability)
        {
            Probability = probability;
        }

        public double Probability { get; }
    }
}
