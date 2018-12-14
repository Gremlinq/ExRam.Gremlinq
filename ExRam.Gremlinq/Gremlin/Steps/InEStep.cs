using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class InEStep : DerivedLabelNamesStep
    {
        public InEStep(string[] labels) : base(labels)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
