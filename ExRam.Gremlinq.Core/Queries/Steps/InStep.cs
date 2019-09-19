using System;

namespace ExRam.Gremlinq.Core
{
    public sealed class InStep : DerivedLabelNamesStep
    {
        public static readonly InStep NoLabels = new InStep(Array.Empty<string>());

        public InStep(string[] labels) : base(labels)
        {
        }
    }
}
