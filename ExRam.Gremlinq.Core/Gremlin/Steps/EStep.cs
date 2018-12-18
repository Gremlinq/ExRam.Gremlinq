using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class EStep : FullScanStep
    {
        public EStep(object[] ids) : base(ids)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
