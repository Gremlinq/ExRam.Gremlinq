using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class OutEStep : DerivedLabelNamesStep
    {
        public OutEStep(string[] labels) : base(labels)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
