using System;

namespace ExRam.Gremlinq
{
    public sealed class OutStep : DerivedLabelNamesStep
    {
        public OutStep(IGraphModel model, Type type) : base(model, type)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
