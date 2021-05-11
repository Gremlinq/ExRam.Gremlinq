using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class FilterStep : Step
    {
        public FilterStep(ILambda lambda, QuerySemantics? semantics = default) : base(semantics)
        {
            Lambda = lambda;
        }

        public ILambda Lambda { get; }
    }
}
