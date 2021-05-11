using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class CountStep : Step
    {
        public static readonly CountStep Global = new(Scope.Global);
        public static readonly CountStep Local = new(Scope.Local);

        public CountStep(Scope scope, QuerySemantics? semantics = default) : base(semantics)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
