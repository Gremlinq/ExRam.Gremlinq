using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>where(predicate)</c> step that filters by a predicate.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#where-step">Reference Documentation - Where Step</seealso>
    public sealed class WherePredicateStep : Step, IFilterStep
    {
        /// <summary>Represents a <c>by()</c> modulator specifying the member to compare in a <c>where()</c> step.</summary>
        public sealed class ByMemberStep : Step
        {
            /// <summary>Initializes a new instance of <see cref="ByMemberStep"/>.</summary>
            /// <param name="key">The optional member key to compare.</param>
            public ByMemberStep(Key? key = null)
            {
                Key = key;
            }

            /// <summary>Gets the optional member key.</summary>
            public Key? Key { get; }
        }

        /// <summary>Initializes a new instance of <see cref="WherePredicateStep"/> with the specified predicate.</summary>
        /// <param name="predicate">The predicate to filter by.</param>
        public WherePredicateStep(P predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);

            Predicate = predicate;
        }

        /// <summary>Gets the filter predicate.</summary>
        public P Predicate { get; }
    }
}
