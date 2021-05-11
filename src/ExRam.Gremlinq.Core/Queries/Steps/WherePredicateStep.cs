using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class WherePredicateStep : Step, IIsOptimizableInWhere
    {
        public sealed class ByMemberStep : Step
        {
            public ByMemberStep(Key? key = default, QuerySemantics? semantics = default) : base(semantics)
            {
                Key = key;
            }

            public Key? Key { get; }
        }

        public WherePredicateStep(P predicate, QuerySemantics? semantics = default) : base(semantics)
        {
            Predicate = predicate;
        }

        public P Predicate { get; }
    }
}
