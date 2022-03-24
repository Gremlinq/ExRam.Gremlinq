using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class EStep : Step
    {
        public EStep(ImmutableArray<object> ids)
        {
            Ids = ids;
        }

        public ImmutableArray<object> Ids { get; }
    }
}

