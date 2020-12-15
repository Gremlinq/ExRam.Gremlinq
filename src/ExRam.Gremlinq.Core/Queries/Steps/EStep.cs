using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class EStep : Step
    {
        public static readonly EStep Empty = new(ImmutableArray<object>.Empty);

        public EStep(ImmutableArray<object> ids)
        {
            Ids = ids;
        }

        public ImmutableArray<object> Ids { get; }
    }
}

