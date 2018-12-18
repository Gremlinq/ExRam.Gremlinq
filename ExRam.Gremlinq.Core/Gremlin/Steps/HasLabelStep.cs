using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class HasLabelStep : DerivedLabelNamesStep
    {
        public HasLabelStep(string[] labels) : base(labels)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
