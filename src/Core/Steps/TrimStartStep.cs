using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>lTrim()</c> step that removes leading whitespace.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#ltrim-step">Reference Documentation - LTrim Step</seealso>
    public sealed class TrimStartStep : Step
    {
        /// <summary>Gets the global-scoped instance.</summary>
        public static readonly TrimStartStep Global = new(Scope.Global);

        private TrimStartStep(Scope scope)
        {
            Scope = scope;
        }

        /// <summary>Gets the scope.</summary>
        public Scope Scope { get; }
    }
}
