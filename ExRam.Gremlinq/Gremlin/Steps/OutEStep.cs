using System;

namespace ExRam.Gremlinq
{
    public sealed class OutEStep : DerivedLabelNamesStep
    {
        public OutEStep(IGraphModel model, Type type) : base(model, type)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
