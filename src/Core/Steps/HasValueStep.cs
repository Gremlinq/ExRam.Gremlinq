namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class HasValueStep : Step, IFilterStep
    {
        public HasValueStep(object argument)
        {
            ArgumentNullException.ThrowIfNull(argument);

            Argument = argument;
        }

        public object Argument { get; }
    }
}
