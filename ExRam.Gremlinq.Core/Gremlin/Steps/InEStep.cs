using System;
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class InEStep : DerivedLabelNamesStep
    {
        public static readonly InEStep NoLabels = new InEStep(Array.Empty<string>());

        public InEStep(string[] labels) : base(labels)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
