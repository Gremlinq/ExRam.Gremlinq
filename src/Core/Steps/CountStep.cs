using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>count()</c> step that counts traversers in the stream.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#count-step">Reference Documentation - Count Step</seealso>
    public sealed class CountStep : Step
    {
        /// <summary>Gets the global-scoped instance.</summary>
        public static readonly CountStep Global = new(Scope.Global);
        /// <summary>Gets the local-scoped instance.</summary>
        public static readonly CountStep Local = new(Scope.Local);

        /// <summary>Initializes a new instance of <see cref="CountStep"/> with the specified scope.</summary>
        /// <param name="scope">The scope of the count operation.</param>
        public CountStep(Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            Scope = scope;
        }

        /// <summary>Gets the scope of the count operation.</summary>
        public Scope Scope { get; }
    }
}
