namespace ExRam.Gremlinq.Core
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
