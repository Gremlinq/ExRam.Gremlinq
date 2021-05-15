using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class PropertiesStep : Step
    {
        public PropertiesStep(ImmutableArray<string> keys) : base()
        {
            Keys = keys;
        }

        public ImmutableArray<string> Keys { get; }
    }
}
