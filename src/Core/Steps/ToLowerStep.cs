using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>toLower()</c> step that converts a string to lowercase.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#tolower-step">Reference Documentation - ToLower Step</seealso>
    public sealed class ToLowerStep : Step
    {
        /// <summary>Gets the global-scoped instance.</summary>
        public static readonly ToLowerStep Global = new(Scope.Global);

        private ToLowerStep(Scope scope)
        {
            Scope = scope;
        }

        /// <summary>Gets the scope.</summary>
        public Scope Scope { get; }
    }
}
