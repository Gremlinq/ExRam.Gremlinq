using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class DedupStep : Step
    {
        public static readonly DedupStep Local = new(Scope.Local);
        public static readonly DedupStep Global = new(Scope.Global);

        public DedupStep(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
