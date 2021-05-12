namespace ExRam.Gremlinq.Core
{
    public sealed class HasValueStep : Step, IIsOptimizableInWhere
    {
        public HasValueStep(object argument, QuerySemantics? semantics = default) : base(semantics)
        {
            Argument = argument;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new HasValueStep(semantics);

        public object Argument { get; }
    }
}
