using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public sealed class LimitStep : NonTerminalStep
    {
        public LimitStep(int limit)
        {
            Limit = limit;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            yield return new MethodStep.MethodStep1("limit", Limit);
        }

        public int Limit { get; }
    }
}
