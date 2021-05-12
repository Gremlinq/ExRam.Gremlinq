namespace ExRam.Gremlinq.Core
{
    public sealed class HasKeyStep : Step, IIsOptimizableInWhere
    {
        public HasKeyStep(object argument, QuerySemantics? semantics = default) : base(semantics)
        {
            Argument = argument;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new HasKeyStep(semantics);

        public object Argument { get; }
    }
}
