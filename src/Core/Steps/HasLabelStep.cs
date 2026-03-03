using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>hasLabel()</c> step that filters elements by their label.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#has-step">Reference Documentation - Has Step</seealso>
    public sealed class HasLabelStep : DerivedLabelNamesStep, IFilterStep
    {
        public HasLabelStep(ImmutableArray<string> labels) : base(labels)
        {
            if (labels.Length == 0)
                throw new ArgumentException($"{nameof(labels)} may not be empty.");
        }
    }
}
