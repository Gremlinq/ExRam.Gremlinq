namespace ExRam.Gremlinq.Core
{
    public sealed class CoinStep : Step, IIsOptimizableInWhere
    {
        public CoinStep(double probability, QuerySemantics? semantics = default) : base(semantics)
        {
            Probability = probability;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new CoinStep(Probability, semantics);

        public double Probability { get; }
    }
}
