using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>max()</c> step that determines the maximum value.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#max-step">Reference Documentation - Max Step</seealso>
    public sealed class MaxStep : Step
    {
        public static readonly MaxStep Local = new(Scope.Local);
        public static readonly MaxStep Global = new(Scope.Global);

        public MaxStep(Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
