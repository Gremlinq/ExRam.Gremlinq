using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>length()</c> step that returns the length of a string.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#length-step">Reference Documentation - Length Step</seealso>
    public sealed class LengthStep : Step
    {
        public static readonly LengthStep Global = new(Scope.Global);

        private LengthStep(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
