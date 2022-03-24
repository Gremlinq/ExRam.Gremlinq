using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class IsStep : Step, IIsOptimizableInWhere
    {
        public IsStep(P predicate)
        {
            Predicate = predicate;
        }

        public P Predicate { get; }
    }
}
