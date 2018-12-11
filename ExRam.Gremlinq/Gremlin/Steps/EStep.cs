namespace ExRam.Gremlinq
{
    public sealed class EStep : FullScanStep
    {
        public EStep(object[] ids) : base(ids)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}