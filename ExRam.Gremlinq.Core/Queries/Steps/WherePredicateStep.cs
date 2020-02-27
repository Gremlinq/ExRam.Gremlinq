using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    //public sealed class WhereStepLabelAndPredicateStep : Step
    //{
    //    public WhereStepLabelAndPredicateStep(StepLabel stepLabel, P predicate)
    //    {
    //        StepLabel = stepLabel;
    //        Predicate = predicate;
    //    }

    //    public P Predicate { get; }
    //    public StepLabel StepLabel { get; }
    //}

    public sealed class WherePredicateStep : Step
    {
        public sealed class ByMemberStep : Step
        {
            public ByMemberStep(object key)
            {
                Key = key;
            }

            public object Key { get; }
        }

        public WherePredicateStep(P predicate)
        {
            Predicate = predicate;
        }

        public P Predicate { get; }
    }
}
