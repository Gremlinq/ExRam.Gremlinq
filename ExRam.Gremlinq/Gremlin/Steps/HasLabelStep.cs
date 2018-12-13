using System;
using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class HasLabelStep : DerivedLabelNamesStep
    {
        public HasLabelStep(IGraphModel model, Type type) : base(model, type)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
