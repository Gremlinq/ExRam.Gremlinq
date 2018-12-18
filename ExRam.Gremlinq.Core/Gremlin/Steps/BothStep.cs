using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class BothStep : DerivedLabelNamesStep
    {
        public BothStep(string[] labels) : base(labels)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
