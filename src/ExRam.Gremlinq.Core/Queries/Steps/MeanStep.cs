using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class MeanStep : Step
    {
        public static readonly MeanStep Local = new(Scope.Local);
        public static readonly MeanStep Global = new(Scope.Global);

        public MeanStep(Scope scope, QuerySemantics? semantics = default) : base(semantics)
        {
            Scope = scope;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new MeanStep(Scope, semantics);

        public Scope Scope { get; }
    }
}
