using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>properties()</c> step that maps elements to their properties.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#properties-step">Reference Documentation - Properties Step</seealso>
    public sealed class PropertiesStep : Step
    {
        internal static readonly PropertiesStep All = new (ImmutableArray<string>.Empty);

        public PropertiesStep(ImmutableArray<string> keys)
        {
            Keys = keys;
        }

        public ImmutableArray<string> Keys { get; }
    }
}
