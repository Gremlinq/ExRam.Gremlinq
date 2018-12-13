using System;

namespace ExRam.Gremlinq
{
    public sealed class BothEStep : DerivedLabelNamesStep
    {
        public BothEStep(IGraphModel model, Type type) : base(model, type)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
