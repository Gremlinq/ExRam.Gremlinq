namespace ExRam.Gremlinq.Core
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
