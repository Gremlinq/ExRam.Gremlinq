using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class ValueMapStep : Step
    {
        internal static readonly ValueMapStep All = new (ImmutableArray<string>.Empty);

        public ValueMapStep(ImmutableArray<string> keys)
        {
            Keys = keys;
        }

        public ImmutableArray<string> Keys { get; }
    }
}
