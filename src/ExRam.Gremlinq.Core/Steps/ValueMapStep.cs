using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class ValueMapStep : Step
    {
        public ValueMapStep(ImmutableArray<string> keys) : base()
        {
            Keys = keys;
        }

        public ImmutableArray<string> Keys { get; }
    }
}
