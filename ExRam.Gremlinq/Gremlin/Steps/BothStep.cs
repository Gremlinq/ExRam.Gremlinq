using System;

namespace ExRam.Gremlinq
{
    public sealed class BothStep : DerivedLabelNamesStep
    {
        public BothStep(IGraphModel model, Type type) : base(model, type)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
