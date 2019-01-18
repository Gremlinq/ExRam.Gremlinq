using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class CoinStep : Step
    {
        public double Probability { get; }

        public CoinStep(double probability)
        {
            Probability = probability;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}