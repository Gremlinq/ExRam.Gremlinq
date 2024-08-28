﻿using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class HasLabelStep : DerivedLabelNamesStep, IFilterStep
    {
        public HasLabelStep(ImmutableArray<string> labels) : base(labels)
        {
            if (labels.Length == 0)
                throw new ArgumentException($"{nameof(labels)} may not be empty.");
        }
    }
}
