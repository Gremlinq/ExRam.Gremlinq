using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
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
