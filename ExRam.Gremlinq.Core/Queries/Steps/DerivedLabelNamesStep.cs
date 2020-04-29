using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public abstract class DerivedLabelNamesStep : Step
    {
        protected DerivedLabelNamesStep(ImmutableArray<string> labels)
        {
            Labels = labels;
        }

        public ImmutableArray<string> Labels { get; }
    }
}
