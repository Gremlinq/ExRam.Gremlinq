using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>trim()</c> step that removes leading and trailing whitespace.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#trim-step">Reference Documentation - Trim Step</seealso>
    public sealed class TrimStep : Step
    {
        /// <summary>Gets the global-scoped instance.</summary>
        public static readonly TrimStep Global = new(Scope.Global);

        private TrimStep(Scope scope)
        {
            Scope = scope;
        }

        /// <summary>Gets the scope.</summary>
        public Scope Scope { get; }
    }
}
