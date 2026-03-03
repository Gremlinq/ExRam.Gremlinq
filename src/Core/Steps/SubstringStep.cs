using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>substring()</c> step that returns a substring.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#substring-step">Reference Documentation - Substring Step</seealso>
    public sealed class SubstringStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="SubstringStep"/>.</summary>
        /// <param name="range">The range of the substring.</param>
        /// <param name="scope">The scope of the operation.</param>
        public SubstringStep(Range range, Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            Range = range;
            Scope = scope;
        }

        /// <summary>Gets the substring range.</summary>
        public Range Range { get; }
        /// <summary>Gets the scope.</summary>
        public Scope Scope { get; }
    }
}
