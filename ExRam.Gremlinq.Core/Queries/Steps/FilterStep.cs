namespace ExRam.Gremlinq.Core
{
    public sealed class FilterStep : Step
    {
        public FilterStep(Lambda lambda)
        {
            Lambda = lambda;
        }

        public Lambda Lambda { get; }
    }
}
