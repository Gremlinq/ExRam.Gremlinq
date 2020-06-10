using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class HasPredicateStep : Step
    {
        public HasPredicateStep(Key key, P? predicate = default)
        {
            Key = key;
            Predicate = predicate;
        }

        public Key Key { get; }
        public P? Predicate { get; }
    }
}
