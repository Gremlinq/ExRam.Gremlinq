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

        public sealed class ByTraversalStep : Step, IIsOptimizableInWhere
        {
            public ByTraversalStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
            {
                Traversal = traversal;
            }

            public Traversal Traversal { get; }
        }

        protected FilterStep(SideEffectSemanticsChange sideEffectSemanticsChange) : base(sideEffectSemanticsChange)
        {

        }
    }
}
