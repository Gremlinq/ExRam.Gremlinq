using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class IsStep : Step, IIsOptimizableInWhere
    {
        public IsStep(P predicate, QuerySemantics? semantics = default) : base(semantics)
        {
            Predicate = predicate;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new IsStep(Predicate, semantics);

        public P Predicate { get; }
    }
}
