using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class WherePredicateStep : Step
    {
        public sealed class ByMemberStep : Step
        {
            public ByMemberStep(object? key = default)
            {
                Key = key;
            }

            public object? Key { get; }
        }

        public WherePredicateStep(P predicate)
        {
            Predicate = predicate;
        }

        public P Predicate { get; }
    }
}
