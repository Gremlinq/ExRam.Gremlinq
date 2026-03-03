using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>min()</c> step that determines the minimum value.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#min-step">Reference Documentation - Min Step</seealso>
    public sealed class MinStep : Step
    {
        /// <summary>Gets the local-scoped instance.</summary>
        public static readonly MinStep Local = new(Scope.Local);
        /// <summary>Gets the global-scoped instance.</summary>
        public static readonly MinStep Global = new(Scope.Global);

        /// <summary>Initializes a new instance of <see cref="MinStep"/> with the specified scope.</summary>
        /// <param name="scope">The scope of the min operation.</param>
        public MinStep(Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            Scope = scope;
        }

        /// <summary>Gets the scope of the min operation.</summary>
        public Scope Scope { get; }
    }
}
