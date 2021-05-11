namespace ExRam.Gremlinq.Core
{
    public sealed class TimesStep : Step
    {
        public TimesStep(int count, QuerySemantics? semantics = default) : base(semantics)
        {
            Count = count;
        }

        public int Count { get; }
    }
}
