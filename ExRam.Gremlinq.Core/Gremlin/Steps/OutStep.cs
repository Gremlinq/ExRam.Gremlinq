using System;
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class OutStep : DerivedLabelNamesStep
    {
        public static readonly OutStep NoLabels = new OutStep(Array.Empty<string>());

        public OutStep(string[] labels) : base(labels)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
