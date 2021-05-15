using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class ValuesStep : Step
    {
        public ValuesStep(ImmutableArray<string> keys) : base()
        {
            Keys = keys;
        }

        public ImmutableArray<string> Keys { get; }
    }
}
