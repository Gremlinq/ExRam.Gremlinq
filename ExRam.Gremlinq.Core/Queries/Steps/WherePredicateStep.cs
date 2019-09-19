namespace ExRam.Gremlinq.Core
{
    public sealed class WherePredicateStep : Step
    {
        public WherePredicateStep(P predicate)
        {
            Predicate = predicate;
        }

        public P Predicate { get; }
    }
}
