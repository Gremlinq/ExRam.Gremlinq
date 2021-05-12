using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class HasPredicateStep : Step, IIsOptimizableInWhere
    {
        public HasPredicateStep(Key key, P predicate, QuerySemantics? semantics = default) : base(semantics)
        {
            Key = key;
            Predicate = predicate;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new HasPredicateStep(Key, Predicate, semantics);

        public Key Key { get; }
        public P Predicate { get; }
    }
}
