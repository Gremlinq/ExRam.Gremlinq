using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class ValuesStep : Step
    {
        public ValuesStep(ImmutableArray<string> keys)
        {
            Keys = keys;
        }

        public ImmutableArray<string> Keys { get; }
    }
}
