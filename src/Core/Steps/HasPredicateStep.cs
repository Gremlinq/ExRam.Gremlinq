using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>has(key, predicate)</c> step that filters elements by a property predicate.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#has-step">Reference Documentation - Has Step</seealso>
    public sealed class HasPredicateStep : Step, IFilterStep
    {
        public HasPredicateStep(Key key, P predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);

            Key = key;
            Predicate = predicate;
        }

        public Key Key { get; }
        public P Predicate { get; }
    }
}
