using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class PropertiesStep : Step
    {
        public PropertiesStep(ImmutableArray<string> keys)
        {
            Keys = keys;
        }

        public ImmutableArray<string> Keys { get; }
    }
}
