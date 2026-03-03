using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>has(key, predicate)</c> step that filters elements by a property predicate.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#has-step">Reference Documentation - Has Step</seealso>
    public sealed class HasPredicateStep : Step, IFilterStep
    {
        /// <summary>Initializes a new instance of <see cref="HasPredicateStep"/> with the specified key and predicate.</summary>
        /// <param name="key">The property key to filter on.</param>
        /// <param name="predicate">The predicate the property value must satisfy.</param>
        public HasPredicateStep(Key key, P predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);

            Key = key;
            Predicate = predicate;
        }

        /// <summary>Gets the property key.</summary>
        public Key Key { get; }
        /// <summary>Gets the predicate the property value must satisfy.</summary>
        public P Predicate { get; }
    }
}
