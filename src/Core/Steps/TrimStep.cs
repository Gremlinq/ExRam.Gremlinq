using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>trim()</c> step that removes leading and trailing whitespace.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#trim-step">Reference Documentation - Trim Step</seealso>
    public sealed class TrimStep : Step
    {
        public static readonly TrimStep Global = new(Scope.Global);

        private TrimStep(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
