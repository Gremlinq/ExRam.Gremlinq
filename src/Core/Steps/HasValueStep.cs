namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class HasValueStep : Step, IIsOptimizableInWhere
    {
        public HasValueStep(object argument)
        {
            Argument = argument;
        }

        public object Argument { get; }
    }
}
