using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>dedup()</c> step that removes duplicate traversers.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#dedup-step">Reference Documentation - Dedup Step</seealso>
    public sealed class DedupStep : Step
    {
        /// <summary>Gets the local-scoped instance.</summary>
        public static readonly DedupStep Local = new(Scope.Local);
        /// <summary>Gets the global-scoped instance.</summary>
        public static readonly DedupStep Global = new(Scope.Global);

        /// <summary>Initializes a new instance of <see cref="DedupStep"/> with the specified scope.</summary>
        /// <param name="scope">The scope of the deduplication.</param>
        public DedupStep(Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            Scope = scope;
        }

        /// <summary>Gets the scope of the deduplication.</summary>
        public Scope Scope { get; }
    }
}
