using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>is()</c> step that filters scalar values by a predicate.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#is-step">Reference Documentation - Is Step</seealso>
    public sealed class IsStep : Step, IFilterStep
    {
        /// <summary>Initializes a new instance of <see cref="IsStep"/> with the specified predicate.</summary>
        /// <param name="predicate">The predicate the scalar value must satisfy.</param>
        public IsStep(P predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);

            Predicate = predicate;
        }

        /// <summary>Gets the predicate.</summary>
        public P Predicate { get; }
    }
}
