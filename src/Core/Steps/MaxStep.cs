using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>max()</c> step that determines the maximum value.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#max-step">Reference Documentation - Max Step</seealso>
    public sealed class MaxStep : Step
    {
        /// <summary>Gets the local-scoped instance.</summary>
        public static readonly MaxStep Local = new(Scope.Local);
        /// <summary>Gets the global-scoped instance.</summary>
        public static readonly MaxStep Global = new(Scope.Global);

        /// <summary>Initializes a new instance of <see cref="MaxStep"/> with the specified scope.</summary>
        /// <param name="scope">The scope of the max operation.</param>
        public MaxStep(Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            Scope = scope;
        }

        /// <summary>Gets the scope of the max operation.</summary>
        public Scope Scope { get; }
    }
}
