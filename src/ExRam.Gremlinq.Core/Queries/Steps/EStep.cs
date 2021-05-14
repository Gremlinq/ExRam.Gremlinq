using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class EStep : Step
    {
        public EStep(ImmutableArray<object> ids) : base()
        {
            Ids = ids;
        }

        public ImmutableArray<object> Ids { get; }
    }
}

