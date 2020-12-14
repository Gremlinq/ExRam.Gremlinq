using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class WherePredicateStep : Step, IIsOptimizableInWhere
    {
        public sealed class ByMemberStep : Step
        {
            public ByMemberStep(Key? key = default)
            {
                Key = key;
            }

            public Key? Key { get; }
        }

        public WherePredicateStep(P predicate)
        {
            Predicate = predicate;
        }

        public P Predicate { get; }
    }
}
