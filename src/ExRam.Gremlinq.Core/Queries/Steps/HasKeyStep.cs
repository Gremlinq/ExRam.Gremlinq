namespace ExRam.Gremlinq.Core
{
    public sealed class HasKeyStep : Step, IIsOptimizableInWhere
    {
        public HasKeyStep(object argument)
        {
            Argument = argument;
        }

        public object Argument { get; }
    }
}
