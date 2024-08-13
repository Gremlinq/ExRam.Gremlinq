﻿using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class InStep : DerivedLabelNamesStep
    {
        public static readonly InStep NoLabels = new();

        //TODO: Think about making this private to force use of NoLabels.
        public InStep() : this(ImmutableArray<string>.Empty)
        {
        }

        public InStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
