using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>inject()</c> step that injects additional elements into the traversal stream.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#inject-step">Reference Documentation - Inject Step</seealso>
    public sealed class InjectStep : Step
    {
        public InjectStep(ImmutableArray<object> elements)
        {
            Elements = elements;
        }

        public ImmutableArray<object> Elements { get; }
    }
}
