using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class SelectKeysStep : Step
    {
        public SelectKeysStep(ImmutableArray<Key> keys, QuerySemantics? semantics = default) : base(semantics)
        {
            Keys = keys;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new SelectKeysStep(Keys, semantics);

        public ImmutableArray<Key> Keys { get; }
    }
}
