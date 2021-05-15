using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class InjectStep : Step
    {
        public InjectStep(ImmutableArray<object> elements) : base()
        {
            Elements = elements;
        }

        public ImmutableArray<object> Elements { get; }
    }
}
