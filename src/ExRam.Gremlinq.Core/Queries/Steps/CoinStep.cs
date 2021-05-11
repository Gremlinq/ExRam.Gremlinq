namespace ExRam.Gremlinq.Core
{
    public sealed class CoinStep : Step, IIsOptimizableInWhere
    {
        public CoinStep(double probability, QuerySemantics? semantics = default) : base(semantics)
        {
            Probability = probability;
        }

        public double Probability { get; }
    }
}
