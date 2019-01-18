using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
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
