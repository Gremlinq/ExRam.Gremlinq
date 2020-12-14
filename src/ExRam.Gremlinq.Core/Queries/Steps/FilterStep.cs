using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class FilterStep : Step
    {
        public FilterStep(ILambda lambda)
        {
            Lambda = lambda;
        }

        public ILambda Lambda { get; }
    }
}
