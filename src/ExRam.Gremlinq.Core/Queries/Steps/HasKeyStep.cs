namespace ExRam.Gremlinq.Core
{
    public sealed class HasKeyStep : Step, IIsOptimizableInWhere
    {
        public HasKeyStep(object argument, QuerySemantics? semantics = default) : base(semantics)
        {
            Argument = argument;
        }

        public object Argument { get; }
    }
}
