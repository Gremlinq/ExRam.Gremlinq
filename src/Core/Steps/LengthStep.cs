using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>length()</c> step that returns the length of a string.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#length-step">Reference Documentation - Length Step</seealso>
    public sealed class LengthStep : Step
    {
        /// <summary>Gets the global-scoped instance.</summary>
        public static readonly LengthStep Global = new(Scope.Global);

        private LengthStep(Scope scope)
        {
            Scope = scope;
        }

        /// <summary>Gets the scope.</summary>
        public Scope Scope { get; }
    }
}
