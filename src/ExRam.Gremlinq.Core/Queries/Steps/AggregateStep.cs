using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class AggregateStep : Step
    {
        public AggregateStep(Scope scope, StepLabel stepLabel, QuerySemantics? semantics = default) : base(semantics)
        {
            Scope = scope;
            StepLabel = stepLabel;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new AggregateStep(Scope, StepLabel, semantics);

        public Scope Scope { get; }
        public StepLabel StepLabel { get; }
    }
}
