using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class DedupStep : Step
    {
        public static readonly DedupStep Local = new(Scope.Local);
        public static readonly DedupStep Global = new(Scope.Global);

        public DedupStep(Scope scope, QuerySemantics? semantics = default) : base(semantics)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
