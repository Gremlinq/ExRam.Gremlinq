using System;

namespace ExRam.Gremlinq
{
    public sealed class HasLabelStep : DerivedLabelNamesStep
    {
        public HasLabelStep(IGraphModel model, Type type) : base(model, type)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
