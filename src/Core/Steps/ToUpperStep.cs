using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>toUpper()</c> step that converts a string to uppercase.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#toupper-step">Reference Documentation - ToUpper Step</seealso>
    public sealed class ToUpperStep : Step
    {
        public static readonly ToUpperStep Global = new(Scope.Global);

        private ToUpperStep(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
