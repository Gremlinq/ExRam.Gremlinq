using System;

namespace ExRam.Gremlinq
{
    public sealed class InEStep : DerivedLabelNamesStep
    {
        public InEStep(IGraphModel model, Type type) : base(model, type)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
