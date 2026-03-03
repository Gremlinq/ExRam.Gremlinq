using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Base class for steps that carry a set of label names derived from type parameters.</summary>
    public abstract class DerivedLabelNamesStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="DerivedLabelNamesStep"/> with the specified labels.</summary>
        /// <param name="labels">The label names derived from type parameters.</param>
        protected DerivedLabelNamesStep(ImmutableArray<string> labels)
        {
            Labels = labels;
        }

        /// <summary>Gets the label names.</summary>
        public ImmutableArray<string> Labels { get; }
    }
}
