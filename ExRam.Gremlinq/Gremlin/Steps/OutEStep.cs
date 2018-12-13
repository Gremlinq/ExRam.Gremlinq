using System;
using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class OutEStep : DerivedLabelNamesStep
    {
        public OutEStep(IGraphModel model, Type type) : base(model, type)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
