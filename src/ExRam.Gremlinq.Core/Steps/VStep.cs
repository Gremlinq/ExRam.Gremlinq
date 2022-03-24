using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class VStep : Step
    {
        public VStep(ImmutableArray<object> ids)
        {
            Ids = ids;
        }

        public ImmutableArray<object> Ids { get; }
    }
}

