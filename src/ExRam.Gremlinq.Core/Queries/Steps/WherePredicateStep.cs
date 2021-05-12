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

            public override Step OverrideQuerySemantics(QuerySemantics semantics) => new ByMemberStep(Key, semantics);

            public Key? Key { get; }
        }

        public WherePredicateStep(P predicate, QuerySemantics? semantics = default) : base(semantics)
        {
            Predicate = predicate;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new WherePredicateStep(Predicate, semantics);

        public P Predicate { get; }
    }
}
