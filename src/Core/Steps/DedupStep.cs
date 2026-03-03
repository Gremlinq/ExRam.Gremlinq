using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>dedup()</c> step that removes duplicate traversers.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#dedup-step">Reference Documentation - Dedup Step</seealso>
    public sealed class DedupStep : Step
    {
        public static readonly DedupStep Local = new(Scope.Local);
        public static readonly DedupStep Global = new(Scope.Global);

        public DedupStep(Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
