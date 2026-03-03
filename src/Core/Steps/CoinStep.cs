namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>coin()</c> step that randomly filters traversers with a given probability.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#coin-step">Reference Documentation - Coin Step</seealso>
    public sealed class CoinStep : Step, IFilterStep
    {
        public CoinStep(double probability)
        {
            Probability = probability;
        }

        public double Probability { get; }
    }
}
