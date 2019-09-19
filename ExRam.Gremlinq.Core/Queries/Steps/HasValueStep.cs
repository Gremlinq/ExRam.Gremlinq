namespace ExRam.Gremlinq.Core
{
    public sealed class HasValueStep : Step
    {
        public HasValueStep(object argument)
        {
            Argument = argument;
        }

        public object Argument { get; }
    }
}
