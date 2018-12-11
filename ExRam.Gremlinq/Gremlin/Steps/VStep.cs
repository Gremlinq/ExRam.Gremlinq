namespace ExRam.Gremlinq
{
    public sealed class VStep : FullScanStep
    {
        public VStep(object[] ids) : base(ids)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
