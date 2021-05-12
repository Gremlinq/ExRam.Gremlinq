namespace ExRam.Gremlinq.Core
{
    public sealed class TimesStep : Step
    {
        public TimesStep(int count, QuerySemantics? semantics = default) : base(semantics)
        {
            Count = count;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new TimesStep(Count, semantics);

        public int Count { get; }
    }
}
