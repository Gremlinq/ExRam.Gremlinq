using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class SelectKeysStep : Step
    {
        public SelectKeysStep(ImmutableArray<Key> keys) : base()
        {
            Keys = keys;
        }

        public ImmutableArray<Key> Keys { get; }
    }
}
