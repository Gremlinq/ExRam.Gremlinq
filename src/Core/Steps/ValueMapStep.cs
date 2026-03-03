using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>valueMap()</c> step that maps elements to a dictionary of property keys and values.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#valuemap-step">Reference Documentation - ValueMap Step</seealso>
    public sealed class ValueMapStep : Step
    {
        internal static readonly ValueMapStep All = new (ImmutableArray<string>.Empty);

        public ValueMapStep(ImmutableArray<string> keys)
        {
            Keys = keys;
        }

        public ImmutableArray<string> Keys { get; }
    }
}
