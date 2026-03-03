using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>sum()</c> step that computes the sum of values.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#sum-step">Reference Documentation - Sum Step</seealso>
    public sealed class SumStep : Step
    {
        /// <summary>Gets the local-scoped instance.</summary>
        public static readonly SumStep Local = new(Scope.Local);
        /// <summary>Gets the global-scoped instance.</summary>
        public static readonly SumStep Global = new(Scope.Global);

        /// <summary>Initializes a new instance of <see cref="SumStep"/> with the specified scope.</summary>
        /// <param name="scope">The scope of the sum operation.</param>
        public SumStep(Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            Scope = scope;
        }

        /// <summary>Gets the scope of the sum operation.</summary>
        public Scope Scope { get; }
    }
}
