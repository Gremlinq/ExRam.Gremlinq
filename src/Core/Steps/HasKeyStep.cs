namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class HasKeyStep : Step, IFilterStep
    {
        public HasKeyStep(object argument)
        {
            ArgumentNullException.ThrowIfNull(argument);

            Argument = argument;
        }

        public object Argument { get; }
    }
}
