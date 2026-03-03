using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>rTrim()</c> step that removes trailing whitespace.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#rtrim-step">Reference Documentation - RTrim Step</seealso>
    public sealed class TrimEndStep : Step
    {
        public static readonly TrimEndStep Global = new(Scope.Global);

        private TrimEndStep(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
