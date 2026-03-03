using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>select(keys)</c> step that selects labeled steps by their string keys.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#select-step">Reference Documentation - Select Step</seealso>
    public sealed class SelectKeysStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="SelectKeysStep"/> with a single key.</summary>
        /// <param name="key">The key to select.</param>
        public SelectKeysStep(Key key) : this(ImmutableArray.Create(key))
        {
        }

        /// <summary>Initializes a new instance of <see cref="SelectKeysStep"/> with the specified keys.</summary>
        /// <param name="keys">The keys to select.</param>
        public SelectKeysStep(ImmutableArray<Key> keys)
        {
            Keys = keys;
        }

        /// <summary>Gets the keys to select.</summary>
        public ImmutableArray<Key> Keys { get; }
    }
}
