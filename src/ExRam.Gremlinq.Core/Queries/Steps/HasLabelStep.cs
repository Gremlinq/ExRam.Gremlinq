using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class HasLabelStep : DerivedLabelNamesStep, IIsOptimizableInWhere
    {
        public HasLabelStep(ImmutableArray<string> labels, QuerySemantics? semantics = default) : base(labels, semantics)
        {
            if (labels.Length == 0)
                throw new ArgumentException($"{nameof(labels)} may not be empty.");
        }
    }
}
