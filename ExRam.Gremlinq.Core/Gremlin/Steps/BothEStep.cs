using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class BothEStep : DerivedLabelNamesStep
    {
        public BothEStep(string[] labels) : base(labels)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
