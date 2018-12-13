using System;

namespace ExRam.Gremlinq
{
    public sealed class InStep : DerivedLabelNamesStep
    {
        public InStep(IGraphModel model, Type type) : base(model, type)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
