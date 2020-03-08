using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class HasPredicateStep : Step
    {
        public HasPredicateStep(object key, P? predicate = default)
        {
            Key = key;
            Predicate = predicate;
        }

        public object Key { get; }
        public P? Predicate { get; }
    }
}
