using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class TrimStartStep : Step
    {
        public static readonly TrimStartStep Global = new(Scope.Global);

        private TrimStartStep(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
