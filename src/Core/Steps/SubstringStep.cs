namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class SubstringStep : Step
    {
        public SubstringStep(Range range)
        {
            Range = range;
        }

        public Range Range { get; }
    }
}
