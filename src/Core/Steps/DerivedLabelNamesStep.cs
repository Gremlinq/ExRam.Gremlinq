using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Base class for steps that carry a set of label names derived from type parameters.</summary>
    public abstract class DerivedLabelNamesStep : Step
    {
        protected DerivedLabelNamesStep(ImmutableArray<string> labels)
        {
            Labels = labels;
        }

        public ImmutableArray<string> Labels { get; }
    }
}
