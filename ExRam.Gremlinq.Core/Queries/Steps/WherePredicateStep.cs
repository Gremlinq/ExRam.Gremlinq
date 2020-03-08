using Gremlin.Net.Process.Traversal;
using NullGuard;

namespace ExRam.Gremlinq.Core
{
    public sealed class WherePredicateStep : Step
    {
        public sealed class ByMemberStep : Step
        {
            public ByMemberStep([AllowNull] object? key = default)
            {
                Key = key;
            }

            [AllowNull]
            public object? Key { get; }
        }

        public WherePredicateStep(P predicate)
        {
            Predicate = predicate;
        }

        public P Predicate { get; }
    }
}
