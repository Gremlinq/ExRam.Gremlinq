using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>concat()</c> step with traversal arguments.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#concat-step">Reference Documentation - Concat Step</seealso>
    public sealed class ConcatTraversalsStep : Step
    {
        public ConcatTraversalsStep(ImmutableArray<Traversal> traversals)
        {
            Traversals = traversals;
        }

        public ImmutableArray<Traversal> Traversals { get; }
    }
}
