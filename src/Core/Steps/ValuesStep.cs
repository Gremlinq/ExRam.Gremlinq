using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>values()</c> step that maps elements to their property values.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#values-step">Reference Documentation - Values Step</seealso>
    public sealed class ValuesStep : Step
    {
        internal static readonly ValuesStep All = new (ImmutableArray<string>.Empty);

        public ValuesStep(ImmutableArray<string> keys)
        {
            Keys = keys;
        }

        public ImmutableArray<string> Keys { get; }
    }
}
