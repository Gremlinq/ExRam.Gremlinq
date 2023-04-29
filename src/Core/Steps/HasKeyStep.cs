namespace ExRam.Gremlinq.Core.Steps
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
