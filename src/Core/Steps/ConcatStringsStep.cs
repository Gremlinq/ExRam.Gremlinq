using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>concat()</c> step with string constant arguments.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#concat-step">Reference Documentation - Concat Step</seealso>
    public sealed class ConcatStringsStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="ConcatStringsStep"/> with the specified string constants.</summary>
        /// <param name="strings">The string constants to concatenate.</param>
        public ConcatStringsStep(ImmutableArray<string> strings)
        {
            Strings = strings;
        }

        /// <summary>Gets the string constants to concatenate.</summary>
        public ImmutableArray<string> Strings { get; }
    }
}
