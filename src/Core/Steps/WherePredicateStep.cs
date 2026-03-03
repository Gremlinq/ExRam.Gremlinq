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
            public ByMemberStep(Key? key = null)
            {
                Key = key;
            }

            public Key? Key { get; }
        }

        public WherePredicateStep(P predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);

            Predicate = predicate;
        }

        public P Predicate { get; }
    }
}
