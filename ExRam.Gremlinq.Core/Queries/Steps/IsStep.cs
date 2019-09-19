namespace ExRam.Gremlinq.Core
{
    public sealed class IsStep : Step
    {
        public IsStep(object argument)
        {
            Argument = argument;
        }

        public object Argument { get; }
    }
}
