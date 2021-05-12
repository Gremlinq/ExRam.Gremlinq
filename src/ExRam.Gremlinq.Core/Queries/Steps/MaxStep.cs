using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class MaxStep : Step
    {
        public static readonly MaxStep Local = new(Scope.Local);
        public static readonly MaxStep Global = new(Scope.Global);

        public MaxStep(Scope scope, QuerySemantics? semantics = default) : base(semantics)
        {
            Scope = scope;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new MaxStep(Scope, semantics);

        public Scope Scope { get; }
    }
}
