using System;
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class BothEStep : DerivedLabelNamesStep
    {
        public static readonly BothEStep NoLabels = new BothEStep(Array.Empty<string>());

        public BothEStep(string[] labels) : base(labels)
        {
        }
    }
}
