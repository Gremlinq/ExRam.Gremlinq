namespace ExRam.Gremlinq.Core
{
    public sealed class HasKeyStep : Step
    {
        public HasKeyStep(object argument)
        {
            Argument = argument;
        }

        public object Argument { get; }
    }
}