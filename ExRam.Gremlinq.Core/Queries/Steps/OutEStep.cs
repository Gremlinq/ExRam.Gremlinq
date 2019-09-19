using System;

namespace ExRam.Gremlinq.Core
{
    public sealed class OutEStep : DerivedLabelNamesStep
    {
        public static readonly OutEStep NoLabels = new OutEStep(Array.Empty<string>());

        public OutEStep(string[] labels) : base(labels)
        {
        }
    }
}
