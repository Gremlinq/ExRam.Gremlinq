using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>min()</c> step that determines the minimum value.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#min-step">Reference Documentation - Min Step</seealso>
    public sealed class MinStep : Step
    {
        public static readonly MinStep Local = new(Scope.Local);
        public static readonly MinStep Global = new(Scope.Global);

        public MinStep(Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
