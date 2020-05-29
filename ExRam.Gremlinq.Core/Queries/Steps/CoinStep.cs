namespace ExRam.Gremlinq.Core
{
    public sealed class CoinStep : Step
    {
        public CoinStep(double probability)
        {
            Probability = probability;
        }

        public double Probability { get; }
    }
}
