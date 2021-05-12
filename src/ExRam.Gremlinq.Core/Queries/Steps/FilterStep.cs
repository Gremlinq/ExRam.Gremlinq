using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class FilterStep : Step
    {
        public FilterStep(ILambda lambda, QuerySemantics? semantics = default) : base(semantics)
        {
            Lambda = lambda;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new FilterStep(Lambda, semantics);

        public ILambda Lambda { get; }
    }
}
