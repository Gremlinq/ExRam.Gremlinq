namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class HasValueStep : Step, IIsOptimizableInWhere
    {
        public HasValueStep(object argument) : base()
        {
            Argument = argument;
        }

        public object Argument { get; }
    }
}
