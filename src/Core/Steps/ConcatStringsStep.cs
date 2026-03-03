using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>concat()</c> step with string constant arguments.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#concat-step">Reference Documentation - Concat Step</seealso>
    public sealed class ConcatStringsStep : Step
    {
        public ConcatStringsStep(ImmutableArray<string> strings)
        {
            Strings = strings;
        }

        public ImmutableArray<string> Strings { get; }
    }
}
