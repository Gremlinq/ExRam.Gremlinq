using System;

namespace ExRam.Gremlinq.Core
{
    public sealed class BothStep : DerivedLabelNamesStep
    {
        public static readonly BothStep NoLabels = new BothStep(Array.Empty<string>());

        public BothStep(string[] labels) : base(labels)
        {
        }
    }
}
