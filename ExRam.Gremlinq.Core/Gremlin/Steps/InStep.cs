using System;
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class InStep : DerivedLabelNamesStep
    {
        public static readonly InStep NoLabels = new InStep(Array.Empty<string>());

        public InStep(string[] labels) : base(labels)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
