using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class ValueMapStep : Step
    {
        public ValueMapStep(ImmutableArray<string> keys)
        {
            Keys = keys;
        }

        public ImmutableArray<string> Keys { get; }
    }
}
