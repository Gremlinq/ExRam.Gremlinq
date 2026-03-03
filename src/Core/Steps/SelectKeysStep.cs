using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>select(keys)</c> step that selects labeled steps by their string keys.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#select-step">Reference Documentation - Select Step</seealso>
    public sealed class SelectKeysStep : Step
    {
        public SelectKeysStep(Key key) : this(ImmutableArray.Create(key))
        {
        }

        public SelectKeysStep(ImmutableArray<Key> keys)
        {
            Keys = keys;
        }

        public ImmutableArray<Key> Keys { get; }
    }
}
