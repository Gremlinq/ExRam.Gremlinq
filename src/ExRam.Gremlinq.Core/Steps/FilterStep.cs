using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public abstract class FilterStep : Step
    {
        public sealed class ByLambdaStep : FilterStep
        {
            public ByLambdaStep(ILambda lambda) : base(SideEffectSemanticsChange.Write)
            {
                Lambda = lambda;
            }

            public ILambda Lambda { get; }
        }

        protected FilterStep(SideEffectSemanticsChange sideEffectSemanticsChange) : base(sideEffectSemanticsChange)
        {

        }
    }
}
