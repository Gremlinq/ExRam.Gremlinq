using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>count()</c> step that counts traversers in the stream.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#count-step">Reference Documentation - Count Step</seealso>
    public sealed class CountStep : Step
    {
        public static readonly CountStep Global = new(Scope.Global);
        public static readonly CountStep Local = new(Scope.Local);

        public CountStep(Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
