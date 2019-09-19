namespace ExRam.Gremlinq.Core
{
    public sealed class WithoutStrategiesStep : Step
    {
        public string[] ClassNames { get; }

        public WithoutStrategiesStep(string[] classNames)
        {
            ClassNames = classNames;
        }
    }
}
