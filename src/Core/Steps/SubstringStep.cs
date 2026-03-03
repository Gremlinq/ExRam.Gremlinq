using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>substring()</c> step that returns a substring.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#substring-step">Reference Documentation - Substring Step</seealso>
    public sealed class SubstringStep : Step
    {
        public SubstringStep(Range range, Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            Range = range;
            Scope = scope;
        }

        public Range Range { get; }
        public Scope Scope { get; }
    }
}
