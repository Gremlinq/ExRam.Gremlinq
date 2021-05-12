using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class VStep : Step
    {
        public VStep(ImmutableArray<object> ids, QuerySemantics? semantics = default) : base(semantics)
        {
            Ids = ids;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new VStep(Ids, semantics);

        public ImmutableArray<object> Ids { get; }
    }
}

