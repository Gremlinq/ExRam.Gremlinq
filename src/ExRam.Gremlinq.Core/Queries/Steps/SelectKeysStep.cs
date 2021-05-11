using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class SelectKeysStep : Step
    {
        public SelectKeysStep(ImmutableArray<Key> keys, QuerySemantics? semantics = default) : base(semantics)
        {
            Keys = keys;
        }

        public ImmutableArray<Key> Keys { get; }
    }
}
