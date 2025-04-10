using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
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
