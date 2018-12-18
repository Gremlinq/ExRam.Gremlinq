using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class VStep : FullScanStep
    {
        public VStep(object[] ids) : base(ids)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
