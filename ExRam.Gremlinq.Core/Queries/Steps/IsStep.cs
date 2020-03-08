using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class IsStep : Step
    {
        public IsStep(P predicate)
        {
            Predicate = predicate;
        }

        public P Predicate { get; }
    }
}
