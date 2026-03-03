namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>coin()</c> step that randomly filters traversers with a given probability.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#coin-step">Reference Documentation - Coin Step</seealso>
    public sealed class CoinStep : Step, IFilterStep
    {
        /// <summary>Initializes a new instance of <see cref="CoinStep"/> with the specified probability.</summary>
        /// <param name="probability">A value between 0.0 and 1.0 indicating the chance a traverser passes.</param>
        public CoinStep(double probability)
        {
            Probability = probability;
        }

        /// <summary>Gets the probability that a traverser passes the filter.</summary>
        public double Probability { get; }
    }
}
