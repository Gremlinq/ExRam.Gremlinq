using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class ConcatStringsStep : Step
    {
        public ConcatStringsStep(ImmutableArray<string> strings)
        {
            Strings = strings;
        }

        public ImmutableArray<string> Strings { get; }
    }
}
