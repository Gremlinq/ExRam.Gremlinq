using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>values()</c> step that maps elements to their property values.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#values-step">Reference Documentation - Values Step</seealso>
    public sealed class ValuesStep : Step
    {
        internal static readonly ValuesStep All = new (ImmutableArray<string>.Empty);

        /// <summary>Initializes a new instance of <see cref="ValuesStep"/> with the specified property keys.</summary>
        /// <param name="keys">The property keys to retrieve values for.</param>
        public ValuesStep(ImmutableArray<string> keys)
        {
            Keys = keys;
        }

        /// <summary>Gets the property keys.</summary>
        public ImmutableArray<string> Keys { get; }
    }
}
