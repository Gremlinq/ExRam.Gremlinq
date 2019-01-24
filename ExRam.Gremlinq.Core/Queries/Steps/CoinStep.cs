using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class NoneStep : Step
    {
        public static readonly NoneStep Instance = new NoneStep();

        private NoneStep()
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

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
