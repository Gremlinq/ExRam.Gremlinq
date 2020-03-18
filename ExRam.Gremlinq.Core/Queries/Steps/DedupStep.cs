using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class DedupStep : Step
    {
        public static readonly DedupStep Local = new DedupStep(Scope.Local);
        public static readonly DedupStep Global = new DedupStep(Scope.Global);

        public DedupStep(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
