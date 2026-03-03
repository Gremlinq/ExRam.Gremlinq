using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>mean()</c> step that computes the mean value.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#mean-step">Reference Documentation - Mean Step</seealso>
    public sealed class MeanStep : Step
    {
        public static readonly MeanStep Local = new(Scope.Local);
        public static readonly MeanStep Global = new(Scope.Global);

        public MeanStep(Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
