using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>mean()</c> step that computes the mean value.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#mean-step">Reference Documentation - Mean Step</seealso>
    public sealed class MeanStep : Step
    {
        /// <summary>Gets the local-scoped instance.</summary>
        public static readonly MeanStep Local = new(Scope.Local);
        /// <summary>Gets the global-scoped instance.</summary>
        public static readonly MeanStep Global = new(Scope.Global);

        /// <summary>Initializes a new instance of <see cref="MeanStep"/> with the specified scope.</summary>
        /// <param name="scope">The scope of the mean operation.</param>
        public MeanStep(Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            Scope = scope;
        }

        /// <summary>Gets the scope of the mean operation.</summary>
        public Scope Scope { get; }
    }
}
