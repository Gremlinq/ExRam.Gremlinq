using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class OutStep : DerivedLabelNamesStep
    {
        public OutStep(string[] labels) : base(labels)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
