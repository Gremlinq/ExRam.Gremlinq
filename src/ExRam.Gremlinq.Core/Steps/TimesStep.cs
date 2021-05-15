namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class TimesStep : Step
    {
        public TimesStep(int count) : base()
        {
            Count = count;
        }

        public int Count { get; }
    }
}
