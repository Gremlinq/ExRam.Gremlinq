using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class ValueMapStep : Step
    {
        public ValueMapStep(ImmutableArray<string> keys, QuerySemantics? semantics = default) : base(semantics)
        {
            Keys = keys;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new ValueMapStep(Keys, semantics);

        public ImmutableArray<string> Keys { get; }
    }
}
