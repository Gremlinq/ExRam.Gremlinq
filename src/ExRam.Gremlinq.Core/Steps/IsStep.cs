using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class IsStep : Step, IIsOptimizableInWhere
    {
        public IsStep(P predicate) : base()
        {
            Predicate = predicate;
        }

        public P Predicate { get; }
    }
}
