using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class InStep : DerivedLabelNamesStep
    {
        public InStep(string[] labels) : base(labels)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
