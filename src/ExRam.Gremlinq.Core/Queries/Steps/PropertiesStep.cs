using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class PropertiesStep : Step
    {
        public PropertiesStep(ImmutableArray<string> keys, QuerySemantics? semantics = default) : base(semantics)
        {
            Keys = keys;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new PropertiesStep(Keys, semantics);

        public ImmutableArray<string> Keys { get; }
    }
}
