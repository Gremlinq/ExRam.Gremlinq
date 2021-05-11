namespace ExRam.Gremlinq.Core
{
    public sealed class HasValueStep : Step, IIsOptimizableInWhere
    {
        public HasValueStep(object argument, QuerySemantics? semantics = default) : base(semantics)
        {
            Argument = argument;
        }

        public object Argument { get; }
    }
}
