using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>valueMap()</c> step that maps elements to a dictionary of property keys and values.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#valuemap-step">Reference Documentation - ValueMap Step</seealso>
    public sealed class ValueMapStep : Step
    {
        internal static readonly ValueMapStep All = new (ImmutableArray<string>.Empty);

        /// <summary>Initializes a new instance of <see cref="ValueMapStep"/> with the specified property keys.</summary>
        /// <param name="keys">The property keys to include in the map.</param>
        public ValueMapStep(ImmutableArray<string> keys)
        {
            Keys = keys;
        }

        /// <summary>Gets the property keys.</summary>
        public ImmutableArray<string> Keys { get; }
    }
}
