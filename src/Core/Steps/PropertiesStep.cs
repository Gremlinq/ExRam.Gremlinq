using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>properties()</c> step that maps elements to their properties.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#properties-step">Reference Documentation - Properties Step</seealso>
    public sealed class PropertiesStep : Step
    {
        internal static readonly PropertiesStep All = new (ImmutableArray<string>.Empty);

        /// <summary>Initializes a new instance of <see cref="PropertiesStep"/> with the specified property keys.</summary>
        /// <param name="keys">The property keys to retrieve.</param>
        public PropertiesStep(ImmutableArray<string> keys)
        {
            Keys = keys;
        }

        /// <summary>Gets the property keys.</summary>
        public ImmutableArray<string> Keys { get; }
    }
}
