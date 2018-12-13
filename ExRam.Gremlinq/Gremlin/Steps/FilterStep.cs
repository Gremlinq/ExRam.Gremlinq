using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class FilterStep : Step
    {
        public FilterStep(Lambda lambda)
        {
            Lambda = lambda;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Lambda Lambda { get; }
    }
}
